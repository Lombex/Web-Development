title Installing Frontend

@echo off
call npm install && (
    echo node js installed
    call composer install && (
       echo Composer installed
    ) || echo composer install failed
) || echo Nodejs install failed

call npm install @radix-ui/react-checkbox @radix-ui/react-dropdown-menu @radix-ui/react-label @tanstack/react-table

call npm start
