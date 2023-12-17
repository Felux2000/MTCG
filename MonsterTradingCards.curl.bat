@echo off

REM --------------------------------------------------
REM Monster Trading Cards Game
REM --------------------------------------------------
title Monster Trading Cards Game
echo CURL Testing for Monster Trading Cards Game
echo.

REM --------------------------------------------------
echo 1) Create Users (Registration)
REM Create User
curl -i -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"daniel\"}"
echo.
curl -i -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"altenhof\", \"Password\":\"markus\"}"
echo.
curl -i -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"admin\",    \"Password\":\"istrator\"}"
echo.

echo should fail:
curl -i -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"daniel\"}"
echo.
curl -i -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"different\"}"
echo. 
echo.

pause

REM --------------------------------------------------
echo 2) Login Users
echo.
curl -i -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"daniel\"}"
echo.
curl -i -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"altenhof\", \"Password\":\"markus\"}"
echo.
curl -i -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"admin\",    \"Password\":\"istrator\"}"
echo.

echo should fail:
echo.
curl -i -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"different\"}"
echo.
echo.

pause

REM --------------------------------------------------
echo 3) create packages (done by "admin")
echo.
curl -i -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Bearer admin-mtcgToken" -d "[{\"Id\":\"845f0dc7-37d0-426e-994e-43fc3ac83c08\", \"Index\": 1}, {\"Id\":\"99f8f8dc-e25e-4a95-aa2c-782823f36e2a\", \"Index\": 4}, {\"Id\":\"e85e3976-7c86-4d06-9a80-641c2019a79f\", \"Index\": 12}, {\"Id\":\"1cb6ab86-bdb2-47e5-b6e4-68c5ab389334\", \"Index\": 6}, {\"Id\":\"dfdd758f-649c-40f9-ba3a-8657f4b3439f\", \"Index\": 15}]"
echo.																																																																																		 				    
curl -i -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Bearer admin-mtcgToken" -d "[{\"Id\":\"644808c2-f87a-4600-b313-122b02322fd5\", \"Index\": 0}, {\"Id\":\"4a2757d6-b1c3-47ac-b9a3-91deab093531\", \"Index\": 7}, {\"Id\":\"91a6471b-1426-43f6-ad65-6fc473e16f9f\", \"Index\": 3}, {\"Id\":\"4ec8b269-0dfa-4f97-809a-2c63fe2a0025\", \"Index\": 11}, {\"Id\":\"f8043c23-1534-4487-b66b-238e0c3c39b5\", \"Index\": 17}]"
echo.																																																																																		 				    
curl -i -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Bearer admin-mtcgToken" -d "[{\"Id\":\"b017ee50-1c14-44e2-bfd6-2c0c5653a37c\", \"Index\": 2}, {\"Id\":\"d04b736a-e874-4137-b191-638e0ff3b4e7\", \"Index\": 5}, {\"Id\":\"88221cfe-1f84-41b9-8152-8e36c6a354de\", \"Index\": 8}, {\"Id\":\"1d3f175b-c067-4359-989d-96562bfa382c\", \"Index\": 10}, {\"Id\":\"171f6076-4eb5-4a7d-b3f2-2d650cc3d237\", \"Index\": 16}]"
echo.																																																																																		 				    
curl -i -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Bearer admin-mtcgToken" -d "[{\"Id\":\"ed1dc1bc-f0aa-4a0c-8d43-1402189b33c8\", \"Index\": 9}, {\"Id\":\"65ff5f23-1e70-4b79-b3bd-f6eb679dd3b5\", \"Index\": 4}, {\"Id\":\"55ef46c4-016c-4168-bc43-6b9b1e86414f\", \"Index\": 13}, {\"Id\":\"f3fad0f2-a1af-45df-b80d-2e48825773d9\", \"Index\": 18}, {\"Id\":\"8c20639d-6400-4534-bd0f-ae563f11f57a\", \"Index\": 12}]"
echo.																																																																																		 				    
curl -i -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Bearer admin-mtcgToken" -d "[{\"Id\":\"d7d0cb94-2cbf-4f97-8ccf-9933dc5354b8\", \"Index\": 4}, {\"Id\":\"44c82fbc-ef6d-44ab-8c7a-9fb19a0e7c6e\", \"Index\": 12}, {\"Id\":\"2c98cd06-518b-464c-b911-8d787216cddd\", \"Index\": 9}, {\"Id\":\"951e886a-0fbf-425d-8df5-af2ee4830d85\", \"Index\": 2}, {\"Id\":\"dcd93250-25a7-4dca-85da-cad2789f7198\", \"Index\": 5}]"
echo.																																																																																		 				    
curl -i -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Bearer admin-mtcgToken" -d "[{\"Id\":\"b2237eca-0271-43bd-87f6-b22f70d42ca4\", \"Index\": 7}, {\"Id\":\"9e8238a4-8a7a-487f-9f7d-a8c97899eb48\", \"Index\": 3}, {\"Id\":\"d60e23cf-2238-4d49-844f-c7589ee5342e\", \"Index\": 15}, {\"Id\":\"fc305a7a-36f7-4d30-ad27-462ca0445649\", \"Index\": 8}, {\"Id\":\"84d276ee-21ec-4171-a509-c1b88162831c\", \"Index\": 7}]"
echo.
echo.

pause

REM --------------------------------------------------
echo 4) acquire packages kienboec
echo.
curl -i -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer kienboec-mtcgToken" -d ""
echo.
curl -i -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer kienboec-mtcgToken" -d ""
echo.
curl -i -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer kienboec-mtcgToken" -d ""
echo.
curl -i -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer kienboec-mtcgToken" -d ""
echo.
echo should fail (no money):
echo.
curl -i -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer kienboec-mtcgToken" -d ""
echo.
echo.

pause

REM --------------------------------------------------
echo 5) acquire packages altenhof
echo.
curl -i -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer altenhof-mtcgToken" -d ""
echo.
curl -i -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer altenhof-mtcgToken" -d ""
echo.
echo should fail (no package):
echo.
curl -i -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer altenhof-mtcgToken" -d ""
echo.
echo.

pause

REM --------------------------------------------------
echo 6) add new packages
echo.
curl -i -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Bearer admin-mtcgToken" -d "[{\"Id\":\"67f9048f-99b8-4ae4-b866-d8008d00c53d\", \"Index\": 9}, {\"Id\":\"aa9999a0-734c-49c6-8f4a-651864b14e62\", \"Index\": 4}, {\"Id\":\"d6e9c720-9b5a-40c7-a6b2-bc34752e3463\", \"Index\": 13}, {\"Id\":\"02a9c76e-b17d-427f-9240-2dd49b0d3bfd\", \"Index\": 18}, {\"Id\":\"2508bf5c-20d7-43b4-8c77-bc677decadef\", \"Index\": 12}]"
echo.																																																																																		 				    
curl -i -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Bearer admin-mtcgToken" -d "[{\"Id\":\"70962948-2bf7-44a9-9ded-8c68eeac7793\", \"Index\": 4}, {\"Id\":\"74635fae-8ad3-4295-9139-320ab89c2844\", \"Index\": 12}, {\"Id\":\"ce6bcaee-47e1-4011-a49e-5a4d7d4245f3\", \"Index\": 9}, {\"Id\":\"a6fde738-c65a-4b10-b400-6fef0fdb28ba\", \"Index\": 2}, {\"Id\":\"a1618f1e-4f4c-4e09-9647-87e16f1edd2d\", \"Index\": 5}]"
echo.																																																																																		 				    
curl -i -X POST http://localhost:10001/packages --header "Content-Type: application/json" --header "Authorization: Bearer admin-mtcgToken" -d "[{\"Id\":\"2272ba48-6662-404d-a9a1-41a9bed316d9\", \"Index\": 7}, {\"Id\":\"3871d45b-b630-4a0d-8bc6-a5fc56b6a043\", \"Index\": 3}, {\"Id\":\"166c1fd5-4dcb-41a8-91cb-f45dcd57cef3\", \"Index\": 15}, {\"Id\":\"237dbaef-49e3-4c23-b64b-abf5c087b276\", \"Index\": 8}, {\"Id\":\"27051a20-8580-43ff-a473-e986b52f297a\", \"Index\": 7}]"
echo.
echo.

pause

REM --------------------------------------------------
echo 7) acquire newly created packages altenhof
echo.
curl -i -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer altenhof-mtcgToken" -d ""
echo.
curl -i -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer altenhof-mtcgToken" -d ""
echo.
echo should fail (no money):
echo.
curl -i -X POST http://localhost:10001/transactions/packages --header "Content-Type: application/json" --header "Authorization: Bearer altenhof-mtcgToken" -d ""
echo.
echo.

pause

REM --------------------------------------------------
echo 8) show all acquired cards kienboec
echo.
curl -i -X GET http://localhost:10001/cards --header "Authorization: Bearer kienboec-mtcgToken"
echo.
echo should fail (no token)
echo.
curl -i -X GET http://localhost:10001/cards 
echo.
echo.

pause

REM --------------------------------------------------
echo 9) show all acquired cards altenhof
echo.
curl -i -X GET http://localhost:10001/cards --header "Authorization: Bearer altenhof-mtcgToken"
echo.
echo.

pause

REM --------------------------------------------------
echo 10) show unconfigured deck
echo.
curl -i -X GET http://localhost:10001/deck --header "Authorization: Bearer kienboec-mtcgToken"
echo.
curl -i -X GET http://localhost:10001/deck --header "Authorization: Bearer altenhof-mtcgToken"
echo.
echo.

pause

REM --------------------------------------------------
echo 11) configure deck
echo.
curl -i -X PUT http://localhost:10001/deck --header "Content-Type: application/json" --header "Authorization: Bearer kienboec-mtcgToken" -d "[\"845f0dc7-37d0-426e-994e-43fc3ac83c08\", \"99f8f8dc-e25e-4a95-aa2c-782823f36e2a\", \"e85e3976-7c86-4d06-9a80-641c2019a79f\", \"171f6076-4eb5-4a7d-b3f2-2d650cc3d237\"]"
echo.
curl -i -X GET http://localhost:10001/deck --header "Authorization: Bearer kienboec-mtcgToken"
echo.
curl -i -X PUT http://localhost:10001/deck --header "Content-Type: application/json" --header "Authorization: Bearer altenhof-mtcgToken" -d "[\"aa9999a0-734c-49c6-8f4a-651864b14e62\", \"d6e9c720-9b5a-40c7-a6b2-bc34752e3463\", \"d60e23cf-2238-4d49-844f-c7589ee5342e\", \"02a9c76e-b17d-427f-9240-2dd49b0d3bfd\"]"
echo.
curl -i -X GET http://localhost:10001/deck --header "Authorization: Bearer altenhof-mtcgToken"
echo.
echo.
echo should fail and show original from before:
echo.
curl -i -X PUT http://localhost:10001/deck --header "Content-Type: application/json" --header "Authorization: Bearer altenhof-mtcgToken" -d "[\"845f0dc7-37d0-426e-994e-43fc3ac83c08\", \"99f8f8dc-e25e-4a95-aa2c-782823f36e2a\", \"e85e3976-7c86-4d06-9a80-641c2019a79f\", \"171f6076-4eb5-4a7d-b3f2-2d650cc3d237\"]"
echo.
curl -i -X GET http://localhost:10001/deck --header "Authorization: Bearer altenhof-mtcgToken"
echo.
echo.
echo should fail ... only 3 cards set
echo.
curl -i -X PUT http://localhost:10001/deck --header "Content-Type: application/json" --header "Authorization: Bearer altenhof-mtcgToken" -d "[\"aa9999a0-734c-49c6-8f4a-651864b14e62\", \"d6e9c720-9b5a-40c7-a6b2-bc34752e3463\", \"d60e23cf-2238-4d49-844f-c7589ee5342e\"]"
echo.

pause

REM --------------------------------------------------
echo 12) show configured deck 
echo.
curl -i -X GET http://localhost:10001/deck --header "Authorization: Bearer kienboec-mtcgToken"
echo.
curl -i -X GET http://localhost:10001/deck --header "Authorization: Bearer altenhof-mtcgToken"
echo.
echo.

pause

REM --------------------------------------------------
echo 13) show configured deck different representation
echo.
echo kienboec
curl -i -X GET http://localhost:10001/deck?format=plain --header "Authorization: Bearer kienboec-mtcgToken"
echo.
echo.
echo altenhof
curl -i -X GET http://localhost:10001/deck?format=plain --header "Authorization: Bearer altenhof-mtcgToken"
echo.
echo.

pause

REM --------------------------------------------------
echo 14) edit user data
echo.
curl -i -X GET http://localhost:10001/users/kienboec --header "Authorization: Bearer kienboec-mtcgToken"
echo.
curl -i -X GET http://localhost:10001/users/altenhof --header "Authorization: Bearer altenhof-mtcgToken"
echo.
curl -i -X PUT http://localhost:10001/users/kienboec --header "Content-Type: application/json" --header "Authorization: Bearer kienboec-mtcgToken" -d "{\"Name\": \"Kienboeck\",  \"Bio\": \"me playin...\", \"Image\": \":-)\"}"
echo.
curl -i -X PUT http://localhost:10001/users/altenhof --header "Content-Type: application/json" --header "Authorization: Bearer altenhof-mtcgToken" -d "{\"Name\": \"Altenhofer\", \"Bio\": \"me codin...\",  \"Image\": \":-D\"}"
echo.
echo.
echo should fail:
echo.
curl -i -X GET http://localhost:10001/users/kienboec --header "Authorization: Bearer kienboec-mtcgToken"
echo.
curl -i -X GET http://localhost:10001/users/altenhof --header "Authorization: Bearer altenhof-mtcgToken"
echo.
curl -i -X GET http://localhost:10001/users/Altenhofer --header "Authorization: Bearer kienboec-mtcgToken"
echo.
curl -i -X GET http://localhost:10001/users/Kienboeck --header "Authorization: Bearer altenhof-mtcgToken"
echo.
curl -i -X PUT http://localhost:10001/users/Kienboeck --header "Content-Type: application/json" --header "Authorization: Bearer altenhof-mtcgToken" -d "{\"Name\": \"Hoax\",  \"Bio\": \"me playin...\", \"Image\": \":-)\"}"
echo.
curl -i -X PUT http://localhost:10001/users/Altenhofer --header "Content-Type: application/json" --header "Authorization: Bearer kienboec-mtcgToken" -d "{\"Name\": \"Hoax\", \"Bio\": \"me codin...\",  \"Image\": \":-D\"}"
echo.
curl -i -X GET http://localhost:10001/users/someGuy  --header "Authorization: Bearer kienboec-mtcgToken"
echo.
echo.
echo change back:
echo.
curl -i -X PUT http://localhost:10001/users/Kienboeck --header "Content-Type: application/json" --header "Authorization: Bearer Kienboeck-mtcgToken" -d "{\"Name\": \"kienboec\",  \"Bio\": \"me playin...\", \"Image\": \":-)\"}"
echo.
curl -i -X PUT http://localhost:10001/users/Altenhofer --header "Content-Type: application/json" --header "Authorization: Bearer Altenhofer-mtcgToken" -d "{\"Name\": \"altenhof\", \"Bio\": \"me codin...\",  \"Image\": \":-D\"}"
echo.
echo.

pause

REM --------------------------------------------------
echo 15) stats
echo.
curl -i -X GET http://localhost:10001/stats --header "Authorization: Bearer kienboec-mtcgToken"
echo.
curl -i -X GET http://localhost:10001/stats --header "Authorization: Bearer altenhof-mtcgToken"
echo.
echo.

pause

REM --------------------------------------------------
echo 16) scoreboard
echo.
curl -i -X GET http://localhost:10001/scoreboard --header "Authorization: Bearer kienboec-mtcgToken"
echo.
echo.

pause

REM --------------------------------------------------
echo 17) battle
echo.
start /b "kienboec battle" curl -i -X POST http://localhost:10001/battles --header "Authorization: Bearer kienboec-mtcgToken"
start /b "altenhof battle" curl -i -X POST http://localhost:10001/battles --header "Authorization: Bearer altenhof-mtcgToken"
echo.
ping localhost -n 10 >NUL 2>NUL

pause

REM --------------------------------------------------
echo 18) Stats 
echo.
echo kienboec
curl -i -X GET http://localhost:10001/stats --header "Authorization: Bearer kienboec-mtcgToken"
echo.
echo altenhof
curl -i -X GET http://localhost:10001/stats --header "Authorization: Bearer altenhof-mtcgToken"
echo.
echo.

pause

REM --------------------------------------------------
echo 19) scoreboard
echo.
curl -i -X GET http://localhost:10001/scoreboard --header "Authorization: Bearer kienboec-mtcgToken"
echo.
echo.

pause

REM --------------------------------------------------
echo 20) trade
echo check trading deals
echo.
curl -i -X GET http://localhost:10001/tradings --header "Authorization: Bearer kienboec-mtcgToken"
echo.
echo create trading deal
echo.
curl -i -X POST http://localhost:10001/tradings --header "Content-Type: application/json" --header "Authorization: Bearer kienboec-mtcgToken" -d "{\"Id\": \"6cd85277-4590-49d4-b0cf-ba0a921faad0\", \"CardToTrade\": \"1cb6ab86-bdb2-47e5-b6e4-68c5ab389334\", \"Type\": \"monster\", \"MinimumDamage\": 15, \"CoinCost\": 0}"
echo.
echo check trading deals
echo.
curl -i -X GET http://localhost:10001/tradings --header "Authorization: Bearer kienboec-mtcgToken"
echo.
curl -i -X GET http://localhost:10001/tradings --header "Authorization: Bearer altenhof-mtcgToken"
echo.
echo delete trading deals
echo.
curl -i -X DELETE http://localhost:10001/tradings/6cd85277-4590-49d4-b0cf-ba0a921faad0 --header "Authorization: Bearer kienboec-mtcgToken"
echo.
echo.

pause

REM --------------------------------------------------
echo 21) add coins
echo.
curl -i -X PUT http://localhost:10001/coins  --header "Content-Type: application/json" --header "Authorization: Bearer altenhof-mtcgToken" -d "{\"Coins\": 100}"
echo.
curl -i -X PUT http://localhost:10001/coins --header "Content-Type: application/json" --header "Authorization: Bearer kienboec-mtcgToken" -d "{\"Coins\": 100}"
echo.
echo.

pause

REM --------------------------------------------------
echo 22) check trading deals
echo.
curl -i -X GET http://localhost:10001/tradings  --header "Authorization: Bearer kienboec-mtcgToken"
echo.
echo create trading deal (card only)
echo.
curl -i -X POST http://localhost:10001/tradings --header "Content-Type: application/json" --header "Authorization: Bearer kienboec-mtcgToken" -d "{\"Id\": \"6cd85277-4590-49d4-b0cf-ba0a921faad0\", \"CardToTrade\": \"1cb6ab86-bdb2-47e5-b6e4-68c5ab389334\", \"Type\": \"monster\", \"MinimumDamage\": 15, \"CoinCost\": 0}"
echo check trading deals
echo.
curl -i -X GET http://localhost:10001/tradings  --header "Authorization: Bearer kienboec-mtcgToken"
echo.
curl -i -X GET http://localhost:10001/tradings  --header "Authorization: Bearer altenhof-mtcgToken"
echo.
echo try to trade with yourself (should fail)
echo.
curl -i -X POST http://localhost:10001/tradings/6cd85277-4590-49d4-b0cf-ba0a921faad0 --header "Content-Type: application/json" --header "Authorization: Bearer kienboec-mtcgToken" -d "\"4ec8b269-0dfa-4f97-809a-2c63fe2a0025\""
echo.
echo try to trade 
echo.
curl -i -X POST http://localhost:10001/tradings/6cd85277-4590-49d4-b0cf-ba0a921faad0 --header "Content-Type: application/json" --header "Authorization: Bearer altenhof-mtcgToken" -d "\"951e886a-0fbf-425d-8df5-af2ee4830d85\""
echo.
curl -i -X GET http://localhost:10001/tradings --header "Authorization: Bearer kienboec-mtcgToken"
echo.
curl -i -X GET http://localhost:10001/tradings --header "Authorization: Bearer altenhof-mtcgToken"
echo.
echo get user data
echo.
curl -i -X GET http://localhost:10001/users/kienboec --header "Authorization: Bearer kienboec-mtcgToken"
echo.
curl -i -X GET http://localhost:10001/users/altenhof --header "Authorization: Bearer altenhof-mtcgToken"
echo.
echo create trading deal (card and coins)
echo.
curl -i -X POST http://localhost:10001/tradings --header "Content-Type: application/json" --header "Authorization: Bearer altenhof-mtcgToken" -d "{\"Id\": \"6cd85277-4590-49d4-b0cf-ba0a921faad0\", \"CardToTrade\": \"1cb6ab86-bdb2-47e5-b6e4-68c5ab389334\", \"Type\": \"monster\", \"MinimumDamage\": 1, \"CoinCost\": 5}"
echo.
curl -i -X POST http://localhost:10001/tradings/6cd85277-4590-49d4-b0cf-ba0a921faad0 --header "Content-Type: application/json" --header "Authorization: Bearer kienboec-mtcgToken" -d "\"951e886a-0fbf-425d-8df5-af2ee4830d85\""
echo.
echo get user data
echo.
curl -i -X GET http://localhost:10001/users/kienboec --header "Authorization: Bearer kienboec-mtcgToken"
echo.
curl -i -X GET http://localhost:10001/users/altenhof --header "Authorization: Bearer altenhof-mtcgToken"
echo.
echo create trading deal (coins only)
echo.
curl -i -X POST http://localhost:10001/tradings --header "Content-Type: application/json" --header "Authorization: Bearer kienboec-mtcgToken" -d "{\"Id\": \"6cd85277-4590-49d4-b0cf-ba0a921faad0\", \"CardToTrade\": \"1cb6ab86-bdb2-47e5-b6e4-68c5ab389334\", \"Type\": \"monster\", \"MinimumDamage\": 0, \"CoinCost\": 8}"
echo.
echo try to trade 
echo.
curl -i -X POST http://localhost:10001/tradings/6cd85277-4590-49d4-b0cf-ba0a921faad0 --header "Content-Type: application/json" --header "Authorization: Bearer altenhof-mtcgToken" -d ""
echo.
echo get user data
echo.
curl -i -X GET http://localhost:10001/users/kienboec --header "Authorization: Bearer kienboec-mtcgToken"
echo.
curl -i -X GET http://localhost:10001/users/altenhof --header "Authorization: Bearer altenhof-mtcgToken"
echo.

pause

echo.
echo 23) transactions all
echo.
curl -i -X GET http://localhost:10001/transactions --header "Authorization: Bearer kienboec-mtcgToken"
echo.
echo no token (should fail)
echo.
curl -i -X GET http://localhost:10001/transactions
echo.

pause

echo.
echo 24) transactions user
echo.
curl -i -X GET http://localhost:10001/transactions/kienboec --header "Authorization: Bearer kienboec-mtcgToken"
echo.
echo should fail:
echo.
curl -i -X GET http://localhost:10001/transactions/Kienboeck --header "Authorization: Bearer kienboec-mtcgToken"
echo.
curl -i -X GET http://localhost:10001/transactions/kienboec --header "Authorization: Bearer altenhof-mtcgToken"
echo.

REM --------------------------------------------------
echo end...

REM this is approx a sleep 
ping localhost -n 100 >NUL 2>NUL
@echo on
