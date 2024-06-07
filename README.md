### How to Run 
	- Run Docker -> `docker-compose build` and `docker-compose run`
	- Optional - Run migration of database -> `dotnet ef migrations add Initial` and `dotnet ef database update`
	- Run the App

### Additional info
	- I'm well aware that project is not finnished and would need some polishing, but I believe that most of basic functionality required by my Assignment is met in this solution
	- Application can and will be broken if tried hard enough. but straight forward use should be without problem (Problem can occure when trying something I didn't thought about when writing it first and didn't have time to test it with all possible scenarios)
		- Please test this application with simple straight foward usage, eg create tournament, create number of teams (at least 8 or more), optionally add players, choose to generate brackets or groups (not both for same tournament), add some scores to matches and export. Otherwise I'm sure you will bracket it :)
	- Most noticable shortcomming in logic of application is when creating brackets or groups, it is done randomly from available/created teams in database, I meant to have functionality how to do it (with registering teams into tournament and chosing from those team while creating brackets/groups) but didn't have addition hour or two to implement it.
	- Also catching of errors appart from connection to database is done by throwing exceptions without catching them and that is not ideal, I know, I wish I would schedule more time for this, but I would probably spent all that time messing with frontend without real progress.