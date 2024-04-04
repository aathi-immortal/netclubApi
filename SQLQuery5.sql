


truncate table match;
select count(*) from match;
select * from team where league_id = 1;
select count(*) from match where court_id = 49;
select * from match;

SELECT 
    match.match_id,
    match.team1_id,
    Team1.team_name Team1name,
    match.team2_id,
    Team2.team_name Team2name,
    match.start_date start_date,
    match.end_date end_date,
    match.point,
    court.court_name court_name 
FROM 
     match
INNER JOIN 
     team Team1 ON  match.team1_id = Team1.team_id
INNER JOIN
 team Team2 ON  match.team2_id = Team2.team_id
INNER JOIN 
court ON match.court_id=court.court_id
where match.league_id=30;

