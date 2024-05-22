# 1.2. Цепочки методов

### Before

~~~go
package auth

import (
	"github.com/gin-gonic/gin"
	"net/http"
	"strconv"
)

func ShowLoginPage(c *gin.Context) {
	c.HTML(http.StatusOK, "login.html", gin.H{})
}

func Authorize(c *gin.Context) {
	err := c.Request.ParseForm()
	if err != nil {
		c.String(http.StatusBadRequest, "bad request")
		c.Abort()
		return
	}

	userID := GetUserIdWhenAuth(c)
	if userID == 0 {
		c.Redirect(http.StatusPermanentRedirect, "/login")
		c.Abort()
		return
	}

	c.Redirect(http.StatusMovedPermanently, "../enterprises?manager="+strconv.Itoa(int(userID)))
}

const (
	secretKey = "secret"
	userIdKey = "userId"
)

var userInfo = map[string]map[string]interface{}{
	"ismirnov@example.com": {
		"secret": "456",
		"userId": int64(1),
	},
	"mgreen@example.com": {
		"secret": "123",
		"userId": int64(2),
	},
}

func GetUserIdWhenAuth(c *gin.Context) int64 {
	login := c.Request.FormValue("login")

	user, ok := userInfo[login]
	if !ok {
		return 0
	}

	secret, ok := user[secretKey]
	if !ok {
		return 0
	}

	password := c.Request.FormValue("password")

	if secret != password {
		return 0
	}

	userID := user[userIdKey]
	return userID.(int64)
}
~~~

### After

- Удалось изменить сигнатуру функции, добавив параметр `userID`, который требовал вызова функции `GetUserIdWhenAuth`, и тем самым избавившись от цепочки вызовов.

~~~go
func Authorize(c *gin.Context, userID int) {
	if userID == 0 {
		c.Redirect(http.StatusPermanentRedirect, "/login")
		c.Abort()
		return
	}

	c.Redirect(http.StatusMovedPermanently, "../enterprises?manager="+strconv.Itoa(int(userID)))
}
~~~