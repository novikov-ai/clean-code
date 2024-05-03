# 1.3. У метода слишком большой список параметров

### Before

~~~go
func (gc *gpsClient) GetTrackByTrip(c context.Context,
	tripID int64,
	start,
	end float64,
) map[string]interface{} {

	gpsPointsRows, err := gc.st.Query(c, `SELECT longitude, latitude, created_at
FROM gps_point
WHERE trip_id = $1 AND 
      EXTRACT(EPOCH FROM created_at) >= $2 AND 
      EXTRACT(EPOCH FROM created_at) <= $3`,
		tripID, start, end)

	if err != nil {
		return nil
	}

	var coordinatesList []map[string]float64

	points := make([]GPSPoint, 0, 0)
	for gpsPointsRows.Next() {
		var point GPSPoint
		gpsPointsRows.Scan(&point.Longitude, &point.Latitude, &point.Created)

		points = append(points, point)

		coordinatesList = append(coordinatesList, map[string]float64{"lgn": point.Longitude, "lat": point.Latitude})
	}

	response := map[string]interface{}{
		"coordinates": coordinatesList,
	}

	return response
}
~~~

### After

- Добавили структуру `tripStamp`, которая позволила инкапсулировать 3 аргумента и сократить сигнатуру функцию. 

~~~go
type tripStamp struct {
	tripID int64
	start,
	end float64
}

func (gc *gpsClient) GetTrackByTrip(c context.Context, trip tripStamp) map[string]interface{} {

	gpsPointsRows, err := gc.st.Query(c, `SELECT longitude, latitude, created_at
FROM gps_point
WHERE trip_id = $1 AND 
      EXTRACT(EPOCH FROM created_at) >= $2 AND 
      EXTRACT(EPOCH FROM created_at) <= $3`,
	  trip.tripID, trip.start, trip.end)

	if err != nil {
		return nil
	}

	var coordinatesList []map[string]float64

	points := make([]GPSPoint, 0, 0)
	for gpsPointsRows.Next() {
		var point GPSPoint
		gpsPointsRows.Scan(&point.Longitude, &point.Latitude, &point.Created)

		points = append(points, point)

		coordinatesList = append(coordinatesList, map[string]float64{"lgn": point.Longitude, "lat": point.Latitude})
	}

	response := map[string]interface{}{
		"coordinates": coordinatesList,
	}

	return response
}
~~~