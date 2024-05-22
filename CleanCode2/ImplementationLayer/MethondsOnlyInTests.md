# 1.1. Методы, которые используются только в тестах

### Before

~~~go
package server

import (
	"fmt"
	"net/http"
	"net/http/httptest"
	"testing"

	"github.com/nsf/jsondiff"
)

// TESTS FOLLOWS DESIGN
func TestServeHTTP(t *testing.T) {
	server := New(NewInMemoryPlayersStore())
	server.store.RecordWin("james", "") // warm up

	t.Run("registered endpoint: `/players/{name}`", func(t *testing.T) {
		request, _ := http.NewRequest(http.MethodGet, "/players/james", nil)
		response := httptest.NewRecorder()

		server.ServeHTTP(response, request)

		assertStatus(t, response.Code, http.StatusOK)
		assertResponse(t, response.Body.String(), "1")
	})

	t.Run("registered endpoint: `/league`", func(t *testing.T) {
		request, _ := http.NewRequest(http.MethodGet, "/league", nil)
		response := httptest.NewRecorder()

		server.ServeHTTP(response, request)

		assertStatus(t, response.Code, http.StatusOK)
		assertResponse(t, response.Body.String(), `{"players":["james"]}`)
	})
}

func assertResponse(t *testing.T, got, want string) {
	t.Helper()
	if got != want {
		t.Errorf("got %q, want %q", got, want)
	}
}

func assertStatus(t *testing.T, got, want int) {
	t.Helper()
	if got != want {
		t.Errorf("incorect status, got %d, want %d", got, want)
	}
}

func assertGivenLeague(t *testing.T, server *PlayerServer, league, expected string) {
	request, _ := http.NewRequest(http.MethodGet, "/players/league?value="+league, nil)
	response := httptest.NewRecorder()
	server.ServeHTTP(response, request)

	diff, _ := jsondiff.Compare([]byte(expected), []byte(response.Body.String()), &jsondiff.Options{})
	if diff != jsondiff.FullMatch {
		t.Errorf("got: %q, want: %q", response.Body.String(), expected)
	}
}

type StubPlayerStore struct {
	scores map[string]int
}

func (st *StubPlayerStore) GetPlayerScore(name string) int {
	return st.scores[name]
}

func (st *StubPlayerStore) RecordWin(name string, league string) {
	st.scores[name]++
}

func (st *StubPlayerStore) GetPlayers() []string {
	return nil
}

func (st *StubPlayerStore) GetPlayersOfLeague(league string) []string {
	return []string{}
}
~~~

### After

- Вынесли отдельные методы, используемые только в тестах, в место их непосредственного использования.

~~~go
package server

import (
	"net/http"
	"net/http/httptest"
	"testing"
)

// TESTS FOLLOWS DESIGN
func TestServeHTTP(t *testing.T) {
	server := New(NewInMemoryPlayersStore())
	server.store.RecordWin("james", "") // warm up

	t.Run("registered endpoint: `/players/{name}`", func(t *testing.T) {
		request, _ := http.NewRequest(http.MethodGet, "/players/james", nil)
		response := httptest.NewRecorder()

		server.ServeHTTP(response, request)

		if response.Code != http.StatusOK {
			t.Errorf("incorect status, got %d, want %d", response.Code, http.StatusOK)
		}

		if response.Body.String() != "1" {
			t.Errorf("got %q, want %q", response.Body.String(), "1")
		}
	})

	t.Run("registered endpoint: `/league`", func(t *testing.T) {
		request, _ := http.NewRequest(http.MethodGet, "/league", nil)
		response := httptest.NewRecorder()

		server.ServeHTTP(response, request)

		if response.Code != http.StatusOK {
			t.Errorf("incorect status, got %d, want %d", response.Code, http.StatusOK)
		}

		if response.Body.String() != `{"players":["james"]}` {
			t.Errorf("got %q, want %q", response.Body.String(), `{"players":["james"]}`)
		}
	})
}

type StubPlayerStore struct {
	scores map[string]int
}

func (st *StubPlayerStore) GetPlayerScore(name string) int {
	return st.scores[name]
}

func (st *StubPlayerStore) RecordWin(name string, league string) {
	st.scores[name]++
}

func (st *StubPlayerStore) GetPlayers() []string {
	return nil
}

func (st *StubPlayerStore) GetPlayersOfLeague(league string) []string {
	return []string{}
}
~~~