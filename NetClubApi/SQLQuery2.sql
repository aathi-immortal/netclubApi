truncate table match;
select * from match;
select count(*) from match;
select * from team where league_id = 1;
select count(*) from match where court_id = 1;  
select count(*) from match where court_id = 2;  
select count(*) from match where court_id = 3;  
select count(*) from match where court_id = 4;  
select count(*) from match where court_id = 43;
select count(*) from match where court_id = 44;
select count(*) from match where court_id = 49;

1 -> 3
2 -> 3
3 -> 4
4 -> 3
43 -> 3
44 -> 2
49 -> 3

