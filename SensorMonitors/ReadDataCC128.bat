@echo off
title CurrentCost Data Display
color f0
prompt ""
mode con: cols=100 > nul
mode com3: baud=57600 parity=n data=8 stop=1 > nul
echo Displaying data received from com1 serial port. Press Ctrl-C to stop...
echo.
type com3
pause
