
Separated data access logic from controllers. Consider using a repository or service layer

Implemented DTOs(Data Transfer Objects) to represent data sent to and from the API, ensuring that only necessary information is exposed.

Used async and await where appropriate, especially for I/O-bound operations, to improve scalability.

Implemented proper validation for incoming requests using data annotations or FluentValidation.

Added unit tests to cover critical business logic, controllers, and services.

Used appropriate HTTP status codes for responses.

Implemented security best practices such as input validation, proper authentication, and authorization.

Refactored the code to use dependency injection for services or repositories instead of instantiating them directly in controllers.

Implemented consistent error handling and responses throughout the API. Consider using middleware for global error handling

Identified and removed any unused code or dependencies.

Implemented logging to capture important events and errors for better debugging.

Integrate Swagger for automatic API documentation.

There should be these endpoints:

1. `GET /products/getProducts` - gets all products.
2. `GET /products/searchByName/{name}` - finds all products matching the specified name.
3. `GET /products/getProductById/{id}` - gets the project that matches the specified ID - ID is a GUID.
4. `POST /products/createProduct` - creates a new product.
5. `PUT /products/updateProduct/{id}` - updates a product.
6. `DELETE /products/deleteProduct/{id}` - deletes a product and its options.
7. `GET /products/getProductOptions/{productId}/options` - finds all options for a specified product.
8. `GET /products/getProductOptionById/{productId}/options/{id}` - finds the specified product option for the specified product.
9. `POST /products/createProductOption/{productId}/options` - adds a new product option to the specified product.
10. `PUT /products/updateProductOption/{productId}/options/{id}` - updates the specified product option.
11. `DELETE /products/deleteProductOption/{productId}/options/{id}` - deletes the specified product option.



