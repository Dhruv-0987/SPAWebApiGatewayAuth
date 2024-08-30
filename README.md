 ###  API Gateway Auth for Angular SPA using the gateway as a BFF

This is a simple example of how to use the API Gateway as a BFF (Backend for Frontend) to authenticate users in an Angular SPA.

The Identity is handled by IdentityServer using OIDC and OAuth.

The API Gateway is responsible for authenticating the user via invoking the challenge scheme which is in this case is OIDC using the config for the IdentityServer. The Gateway is responsible to fetch the token from the IdentityServer and store it on HttpContext on the server.

There default SignIn scheme is set to Cookies and the default challenge scheme is set to OIDC. The user from the Angular SPA will be redirected to the IdentityServer to authenticate and then redirected back to the Gateway which will redirect it back to the angualr frontend.

There will be a cookie between the Angular SPA and API Gateway which will be used to authenticate the user from Angular to Gateway.

For any authorised request from the Angular which will all be proxied via the gateway via YARP by adding the access token from the IdentityServer on the request headers.

The key thing here is the Angular SPA and the API gateway is not served from the same origin whereas it is usually the case in BFF implementations. This is a use case for specific scenarios where the Angular SPA is served from a different origin than the API Gateway.

Now there is one more trick here, instead of issuing the entire cookie with information about the session or the token which can make it really big and keep in mind the max size for a cookie is 4kb, I have issues a ticket instead which is essentially the key for the cookie stored in cache on the server.

There is a Redis cache store on the server which acts as the sessions store for the cookies. The ticket is issued to the Angular SPA which is then used to fetch the cookie from the cache store on the server. 

The above approach makes sure there is nothing on the browser which can used to make malicious request to the Gateway.

#### F5:

1. Run `docker-compose up` to start the sql server and the redis store
2. Run the Aspier app host which will start up the IdentityServer, the API Gateway and the resource api we want to fetch data from via authroised requests.
3. Serve the Angular app by `ng s -ssl` which will be served on `https://localhost:4200`, here you need to make sure the angular is served on `https` as the cookie will be set on the server as secure and same site which means the browser won't attach the cookie with each request unless you server it on `https`.


