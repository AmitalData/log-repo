update BusinessHours 
set ThursdayFromHour = '08:00:00.0000000', ThursdayToHour ='17:00:00.0000000', 
	FridayFromHour ='08:00:00.0000000', FridayToHour ='12:00:00.0000000',
	SundayFromHour ='08:00:00.0000000', SundayToHour ='17:00:00.0000000'
where Name = 'Business Hours'
		 
			