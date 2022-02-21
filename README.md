# Online Store Application 

## Task

Develop a console application .NET Core that simulates the work of an online store 

### Level Low 


1. Provide the following roles and functionality:

**Role "Guest"** 

- search for goods by name; 
- user account registration; 
- admission to the online store with an account.

**Role "Registered user"** 

- view the list of goods; 
- search for goods by name; 
- creating a new order; 
- ordering or cancellation; 
- review the history of orders and the status of their delivery; 
- setting the status of the order "Received"; 
- change of personal information; 
- sign out of the account.

**Role "Administrator"** 

- view the list of goods; 
- search for a product by name; 
- creating a new order; 
- ordering; 
- viewing and changing personal information of users; 
- adding a new product (name, category, description, cost); 
- change of information about the product; 
- change the status of the order; 
- sign out of the account.
 

2. In the case of creating a new order, the status "New" is automatically set. All other statuses are set manually by the administrator: "Canceled by the administrator", "Payment received", "Sent", "Received", "Completed". Except for "Canceled by user" - set by the user before "Received". Cover the main functionality with Unit-tests. 

3. When designing classes use the principles of S.O.L.I.D. 

4. Use collections to save information instead of a database or file. 

5. Denay invalid user actions. 


### Level Middle

 
1. Implement the Low level. 

2. Build relations between commands and operations under commands  via delegates. 

3. Cover the main functionality with Unit-tests. 

 
### Level Advanced 

 
1. Implement the Middle level. 

2. Use a design pattern (optional). 

3. Use Moq to save information instead of database or file. 

4. Cover all functionality with Unit-tests. 

5. Add new functionality as desired. 

