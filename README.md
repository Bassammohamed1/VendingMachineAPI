FlapKap challenge :
I used C#12 & .Net 8 with EF 8 and microsoft identity.
I implemented all the endpoints that you mentioned in the challenge.
The API has two controllers one for user and it's name is accountcontroller and the other is for products and it's name is productscontroller.
The accountcontroller has the CRUD operation for the user and (deposit, buy and reset) for the buyer user.
The productscontroller has the CRUD operation for the products an the seller can access this endpoints.
The API has three roles admin, seller and buyer.
Admin can access any endpoint, seller can acess all products endpoints, buyer can acess deposit, buy and reset endpoints.
Any one can acess view products, register and login endpoints.
