select * from team where league_id = 30;

select * from match where league_id = 30;

SELECT 
    [dbo].[match].match_id,
    team1_id,
    Team1.team_name Team1name,
    team2_id,
    Team2.team_name Team2name,
    [dbo].[match].start_date start_date,
    [dbo].[match].end_date end_date,
    [dbo].[match].point,
[dbo].[match].team1_point,
[dbo].[match].team1_rating,
[dbo].[match].team2_point,
[dbo].[match].team2_rating,
    [dbo].[court].court_name court_name 
FROM 
     [dbo].[match]
INNER JOIN 
     [dbo].[team] Team1 ON  [dbo].[match].team1_id = Team1.team_id
INNER JOIN
 [dbo].[team] Team2 ON  [dbo].[match].team2_id = Team2.team_id
INNER JOIN 
[dbo].[court] ON [dbo].[match].court_id=[dbo].[court].court_id
where [dbo].[match].league_id=30


select * from match;

delete from match where league_id = 30;