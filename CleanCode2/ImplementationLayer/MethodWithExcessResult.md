# 1.5. Чрезмерный результат. Метод возвращает больше данных, чем нужно вызывающему его компоненту.

### Before

~~~go
func (p *Provider) FetchAllByManagerID(ctx *gin.Context, managerID int64) []models.Enterprise {
	query := `SELECT e.*
FROM enterprise as e
JOIN manager_enterprise as me on me.enterprise_id = e.id
WHERE me.manager_id = $1;
`

	resp, err := p.db.Query(ctx, query, managerID)
	if err != nil {
		fmt.Fprintf(os.Stderr, "unable to proceed query: %v\n", err)
		return []models.Enterprise{}
	}

	var (
		id          int64
		title, city string
		utc         int
		established time.Time
	)

	enterprises := make([]models.Enterprise, 0)
	for resp.Next() {
		err = resp.Scan(&id, &title, &city, &established, &utc)
		if err != nil {
			fmt.Fprintf(os.Stderr, "scan failed: %v\n", err)
			return []models.Enterprise{}
		}

		enterprises = append(enterprises, models.Enterprise{
			ID:          id,
			Title:       title,
			City:        city,
			Established: established.Round(time.Second),
			UTC:         utc,
		})
	}

	return enterprises
}
~~~

### After

- Вызывающий метод никак не работал с полной структурой, поэтому удалось возвращать только слайс айди элементов. 

~~~go
func (p *Provider) FetchAllByManagerID(ctx *gin.Context, managerID int64) []int64 {
	query := `SELECT e.*
FROM enterprise as e
JOIN manager_enterprise as me on me.enterprise_id = e.id
WHERE me.manager_id = $1;
`

	resp, err := p.db.Query(ctx, query, managerID)
	if err != nil {
		fmt.Fprintf(os.Stderr, "unable to proceed query: %v\n", err)
		return []int64{}
	}

	var (
		id          int64
		title, city string
		utc         int
		established time.Time
	)

	enterprisesIDs := make([]int64, 0)
	for resp.Next() {
		err = resp.Scan(&id, &title, &city, &established, &utc)
		if err != nil {
			fmt.Fprintf(os.Stderr, "scan failed: %v\n", err)
			return []int64{}
		}

		enterprisesIDs = append(enterprisesIDs, id)
	}

	return enterprisesIDs
}
~~~
