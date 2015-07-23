@echo off
cd /d "D:\Projects\ParaNovels\Paranovels.Mvc\assets\build"
cd
echo "Minifying RequireJS"
r.js.cmd -o js-public.js & pause
