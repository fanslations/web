@echo off
cd /d "D:\Projects\ParaNovels\Paranovels.Mvc\assets\build"
cd
echo "Minifying Css"
r.js.cmd -o css-public.js & pause
