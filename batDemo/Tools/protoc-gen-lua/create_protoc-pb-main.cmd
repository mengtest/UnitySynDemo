@echo off
set SOURCE_PATH=../GameProto/main
set ADDRES_PATH=../../Assets/ScriptsLua/pb


protoc.exe --proto_path=%SOURCE_PATH% -o %ADDRES_PATH%/md.bytes %SOURCE_PATH%/*.proto

echo "Success!"