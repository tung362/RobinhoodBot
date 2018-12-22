**Bot made for use with Robinhood, a popular zero comission stock trading app**  
https://robinhood.com/

**Robinhood trading bot made using the unofficial robinhood C# api**  
https://github.com/itsff/RobinhoodNet

**Robinhood REST API**  
https://api.robinhood.com/

**Bot made by: Tung Nguyen**  
https://github.com/tung362


**Requirements:**  
Microsoft .NET Framework 4.6


**Disclaimer:**  
I am not responsible for anything that happens. I made this for my own personal use and just wanted to share it.


**How to use:**  
Login credentials
-For each bin/Debug folder in each project folder you want to use open up a file named "Login.login" with any text editor for example notepad
 and type your robinhood username on the first line and password on the second line.
 
 Example:
 RobinhoodBot\_Buy\bin\Debug\Login.login
 1st line Username
 2nd line Password
 
 -If folder is missing set default project, build and run the project on debug
 
 
**Changing stock symbol and settings**  
-Open up the "Setup.cs" file in the "Shared" project and change the values to what you need

Example:
public static string CurrentStockSymbol = "NVCN"; 	Change "NVCN" to the symbol you wish to trade from
public static decimal StopLossPrice = 0.0610m; 		Change "0.0610" to the price you want your stop loss at (leave in the "m"), ONLY USED FOR _StopLoss project


**Running different projects (switching from buying to selling and reverse)**  
-If you want to buy shares click on the project "_BuyPrice" or _Buy and right click, click "Set as StartUp Project" and run
-If you want to sell shares click on the project "_SellPrice" or _Sell and right click, click "Set as StartUp Project" and run
 
 
 **Notes:**  
-_Buy project 		- Buy the shares with all available buying power at last traded price		|		Price: Auto, 	Amount: Auto
-_BuyPrice project 	- Buy the shares with all available buying power at a specified price		|		Price: Manual, 	Amount: Auto
-_Sell project 		- Sell all the avaliable shares at last traded price						|		Price: Auto, 	Amount: Auto
-_SellPrice project - Sell all the avaliable shares at a specified price						|		Price: Manual, 	Amount: Auto

WARNING: NOT TESTED AND ONLY SEMI WORKS, USE AT YOUR OWN RISK
-_StopLoss project 	- Sell all the avaliable shares at last traded price when price drops to a certain level		|		Price: Auto, 	Amount: Auto
