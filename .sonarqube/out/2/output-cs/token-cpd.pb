�%
9C:\Users\ibrah\Downloads\StudyTrackers\backend\Startup.cs
	namespace 	
StudyTracker
 
{ 
public 

class 
Startup 
{ 
public 
IConfiguration 
Configuration +
{, -
get. 1
;1 2
}3 4
private 
readonly 
IWebHostEnvironment ,
_env- 1
;1 2
public 
Startup 
( 
IConfiguration %
configuration& 3
,3 4
IWebHostEnvironment5 H
envI L
)L M
{ 	
Configuration 
= 
configuration )
;) *
_env 
= 
env 
; 
} 	
public 
void 
ConfigureServices %
(% &
IServiceCollection& 8
services9 A
)A B
{ 	
services 
. 
AddControllers #
(# $
)$ %
;% &
services 
. 
AddDbContext !
<! "
StudyDbContext" 0
>0 1
(1 2
options2 9
=>: <
options   
.   
UseMySql    
(    !
Configuration!! !
.!!! "
GetConnectionString!!" 5
(!!5 6
$str!!6 I
)!!I J
,!!J K
ServerVersion"" !
.""! "

AutoDetect""" ,
("", -
Configuration""- :
."": ;
GetConnectionString""; N
(""N O
$str""O b
)""b c
)""c d
)## 
)## 
;## 
services&& 
.&& 
	AddScoped&& 
<&& 
IStudyService&& ,
,&&, -
StudyService&&. :
>&&: ;
(&&; <
)&&< =
;&&= >
services)) 
.)) 
AddCors)) 
()) 
options)) $
=>))% '
{** 
options++ 
.++ 
	AddPolicy++ !
(++! "
$str++" ,
,++, -
builder++. 5
=>++6 8
builder,, 
.,, 
AllowAnyOrigin,, *
(,,* +
),,+ ,
.,,, -
AllowAnyMethod,,- ;
(,,; <
),,< =
.,,= >
AllowAnyHeader,,> L
(,,L M
),,M N
),,N O
;,,O P
}-- 
)-- 
;-- 
services00 
.00 
AddSwaggerGen00 "
(00" #
)00# $
;00$ %
var33 

togglePath33 
=33 
Path33 !
.33! "
Combine33" )
(33) *
_env33* .
.33. /
ContentRootPath33/ >
,33> ?
$str33@ P
,33P Q
$str33R `
)33` a
;33a b
var44 

toggleJson44 
=44 
File44 !
.44! "
ReadAllText44" -
(44- .

togglePath44. 8
)448 9
;449 :
var55 
featureToggles55 
=55  
JsonSerializer55! /
.55/ 0
Deserialize550 ;
<55; <
FeatureToggles55< J
>55J K
(55K L

toggleJson55L V
)55V W
;55W X
services88 
.88 
AddSingleton88 !
(88! "
featureToggles88" 0
)880 1
;881 2
}99 	
public;; 
void;; 
	Configure;; 
(;; 
IApplicationBuilder;; 1
app;;2 5
,;;5 6
IWebHostEnvironment;;7 J
env;;K N
);;N O
{<< 	
if== 
(== 
env== 
.== 
IsDevelopment== !
(==! "
)==" #
)==# $
{>> 
app?? 
.?? %
UseDeveloperExceptionPage?? -
(??- .
)??. /
;??/ 0
app@@ 
.@@ 

UseSwagger@@ 
(@@ 
)@@  
;@@  !
appAA 
.AA 
UseSwaggerUIAA  
(AA  !
)AA! "
;AA" #
}BB 
appDD 
.DD 

UseRoutingDD 
(DD 
)DD 
;DD 
appEE 
.EE 
UseCorsEE 
(EE 
$strEE "
)EE" #
;EE# $
appFF 
.FF 
UseAuthorizationFF  
(FF  !
)FF! "
;FF" #
appHH 
.HH 
UseEndpointsHH 
(HH 
	endpointsHH &
=>HH' )
{II 
	endpointsJJ 
.JJ 
MapControllersJJ (
(JJ( )
)JJ) *
;JJ* +
}KK 
)KK 
;KK 
}LL 	
}MM 
}NN �
GC:\Users\ibrah\Downloads\StudyTrackers\backend\Services\StudyService.cs
	namespace 	
StudyTracker
 
. 
Services 
{ 
public		 

class		 
StudyService		 
:		 
IStudyService		  -
{

 
private 
readonly 
StudyDbContext '
_context( 0
;0 1
public 
StudyService 
( 
StudyDbContext *
context+ 2
)2 3
{ 	
_context 
= 
context 
; 
} 	
public 
async 
Task 
< 
List 
< 

StudyEntry )
>) *
>* +
GetAllAsync, 7
(7 8
)8 9
=>: <
await 
_context 
. 
StudyEntries '
.' (
ToListAsync( 3
(3 4
)4 5
;5 6
public 
async 
Task 
< 

StudyEntry $
>$ %
GetByIdAsync& 2
(2 3
int3 6
id7 9
)9 :
=>; =
await 
_context 
. 
StudyEntries '
.' (
	FindAsync( 1
(1 2
id2 4
)4 5
;5 6
public 
async 
Task 
< 

StudyEntry $
>$ %
CreateAsync& 1
(1 2

StudyEntry2 <
entry= B
)B C
{ 	
_context 
. 
StudyEntries !
.! "
Add" %
(% &
entry& +
)+ ,
;, -
await 
_context 
. 
SaveChangesAsync +
(+ ,
), -
;- .
return 
entry 
; 
} 	
public 
async 
Task 
< 
bool 
> 
DeleteAsync  +
(+ ,
int, /
id0 2
)2 3
{   	
var!! 
entry!! 
=!! 
await!! 
_context!! &
.!!& '
StudyEntries!!' 3
.!!3 4
	FindAsync!!4 =
(!!= >
id!!> @
)!!@ A
;!!A B
if"" 
("" 
entry"" 
=="" 
null"" 
)"" 
return"" %
false""& +
;""+ ,
_context## 
.## 
StudyEntries## !
.##! "
Remove##" (
(##( )
entry##) .
)##. /
;##/ 0
await$$ 
_context$$ 
.$$ 
SaveChangesAsync$$ +
($$+ ,
)$$, -
;$$- .
return%% 
true%% 
;%% 
}&& 	
}'' 
}(( �
HC:\Users\ibrah\Downloads\StudyTrackers\backend\Services\IStudyService.cs
	namespace 	
StudyTracker
 
. 
Services 
{ 
public 

	interface 
IStudyService "
{ 
Task		 
<		 
List		 
<		 

StudyEntry		 
>		 
>		 
GetAllAsync		 *
(		* +
)		+ ,
;		, -
Task

 
<

 

StudyEntry

 
>

 
GetByIdAsync

 %
(

% &
int

& )
id

* ,
)

, -
;

- .
Task 
< 

StudyEntry 
> 
CreateAsync $
($ %

StudyEntry% /
entry0 5
)5 6
;6 7
Task 
< 
bool 
> 
DeleteAsync 
( 
int "
id# %
)% &
;& '
} 
} �

9C:\Users\ibrah\Downloads\StudyTrackers\backend\Program.cs
	namespace 	
StudyTracker
 
{ 
public 

class 
Program 
{ 
public 
static 
void 
Main 
(  
string  &
[& '
]' (
args) -
)- .
{		 	
CreateHostBuilder 
( 
args "
)" #
.# $
Build$ )
() *
)* +
.+ ,
Run, /
(/ 0
)0 1
;1 2
} 	
public 
static 
IHostBuilder "
CreateHostBuilder# 4
(4 5
string5 ;
[; <
]< =
args> B
)B C
=>D F
Host 
.  
CreateDefaultBuilder %
(% &
args& *
)* +
. $
ConfigureWebHostDefaults )
() *

webBuilder* 4
=>5 7
{ 

webBuilder 
. 

UseStartup )
<) *
Startup* 1
>1 2
(2 3
)3 4
;4 5
} 
) 
; 
} 
} �
CC:\Users\ibrah\Downloads\StudyTrackers\backend\Models\StudyEntry.cs
	namespace 	
StudyTracker
 
. 
Models 
{ 
public 

class 

StudyEntry 
{ 
public 
int 
Id 
{ 
get 
; 
set  
;  !
}" #
public 
string 
? 
Subject 
{  
get! $
;$ %
set& )
;) *
}+ ,
public 
int 
DurationInMinutes $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public 
DateTime 
	Timestamp !
{" #
get$ '
;' (
set) ,
;, -
}. /
=0 1
DateTime2 :
.: ;
Now; >
;> ?
} 
} �
GC:\Users\ibrah\Downloads\StudyTrackers\backend\Models\FeatureToggles.cs
	namespace 	
StudyTracker
 
. 
Models 
{ 
public 

class 
FeatureToggles 
{ 
public 
bool 
EnableStudyStats $
{% &
get' *
;* +
set, /
;/ 0
}1 2
public 
bool  
EnableAdvancedExport (
{) *
get+ .
;. /
set0 3
;3 4
}5 6
} 
} �
EC:\Users\ibrah\Downloads\StudyTrackers\backend\Data\StudyDbContext.cs
	namespace 	
StudyTracker
 
. 
Data 
{ 
public 

class 
StudyDbContext 
:  !
	DbContext" +
{ 
public 
StudyDbContext 
( 
DbContextOptions .
<. /
StudyDbContext/ =
>= >
options? F
)F G
:		 
base		 
(		 
options		 
)		 
{		 
}		 
public 
DbSet 
< 

StudyEntry 
>  
StudyEntries! -
{. /
get0 3
;3 4
set5 8
;8 9
}: ;
} 
} �
MC:\Users\ibrah\Downloads\StudyTrackers\backend\Controllers\StudyController.cs
	namespace 	
StudyTracker
 
. 
Controllers "
{ 
[ 
ApiController 
] 
[		 
Route		 

(		
 
$str		 
)		 
]		 
public

 

class

 
StudyController

  
:

! "
ControllerBase

# 1
{ 
private 
readonly 
IStudyService &
_service' /
;/ 0
public 
StudyController 
( 
IStudyService ,
service- 4
)4 5
{ 	
_service 
= 
service 
; 
} 	
[ 	
HttpGet	 
] 
public 
async 
Task 
< 
IActionResult '
>' (
GetAll) /
(/ 0
)0 1
=>2 4
Ok 
( 
await 
_service 
. 
GetAllAsync )
() *
)* +
)+ ,
;, -
[ 	
HttpGet	 
( 
$str 
) 
] 
public 
async 
Task 
< 
IActionResult '
>' (
GetById) 0
(0 1
int1 4
id5 7
)7 8
{ 	
var 
entry 
= 
await 
_service &
.& '
GetByIdAsync' 3
(3 4
id4 6
)6 7
;7 8
return 
entry 
== 
null  
?! "
NotFound# +
(+ ,
), -
:. /
Ok0 2
(2 3
entry3 8
)8 9
;9 :
} 	
[ 	
HttpPost	 
] 
public 
async 
Task 
< 
IActionResult '
>' (
Create) /
(/ 0
[0 1
FromBody1 9
]9 :

StudyEntry; E
entryF K
)K L
{   	
if!! 
(!! 
!!! 

ModelState!! 
.!! 
IsValid!! #
)!!# $
return!!% +

BadRequest!!, 6
(!!6 7

ModelState!!7 A
)!!A B
;!!B C
var"" 
created"" 
="" 
await"" 
_service""  (
.""( )
CreateAsync"") 4
(""4 5
entry""5 :
)"": ;
;""; <
return## 
CreatedAtAction## "
(##" #
nameof### )
(##) *
GetById##* 1
)##1 2
,##2 3
new##4 7
{##8 9
id##: <
=##= >
created##? F
.##F G
Id##G I
}##J K
,##K L
created##M T
)##T U
;##U V
}$$ 	
[&& 	

HttpDelete&&	 
(&& 
$str&& 
)&& 
]&& 
public'' 
async'' 
Task'' 
<'' 
IActionResult'' '
>''' (
Delete'') /
(''/ 0
int''0 3
id''4 6
)''6 7
{(( 	
var)) 
deleted)) 
=)) 
await)) 
_service))  (
.))( )
DeleteAsync))) 4
())4 5
id))5 7
)))7 8
;))8 9
return** 
deleted** 
?** 
	NoContent** &
(**& '
)**' (
:**) *
NotFound**+ 3
(**3 4
)**4 5
;**5 6
}++ 	
},, 
}-- 