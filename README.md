# TestRedirects

This is a simple AspDotNet server with a couple client projects to check how browsers,
the Net Framework HttpClient and the Net 5.0 HttpClient react when the response from
the server is one of the redirect responses (300, 301, 302, 303, 307 or 308).

This started because I ported a networking client from Net Framework to Net 5.0
and it compiled without error, but it didn't work any more.

I then found an [open issue](https://github.com/dotnet/runtime/issues/28998)
in the [dotnet/runtime](https://github.com/dotnet/runtime) project
and put in effort to fix it.

#### Table of contents
[Summary](#h01)<br>
[How I got the data](#h02)<br>
[Firefox](#h03)<br>
[Chrome/Edge](#h04)<br>
[Net Framework](#h05)<br>
[Net 5.0](#h06)<br>
[Donation](#h07)<br>

## Summary
<a name="h01" />

(Ignoring the current behavior of Net 5.0, since that's what I'm looking at changing.)

The only times a redirect seems to change method to a GET is:

- On a 303 response, except the HEAD will remain HEAD.
- POST also changes to GET on a 301 or 302.
- Net Framework also changes all except HEAD to GET on a 300.
- And I said I would ignore it, but Net 5.0 also changes POST to GET on a 300.

## How I got the data
<a name="h02" />

For each of the browsers, I opened the swagger page in that browser.
I went through each of the HTTP methods and for each method tried the "initial" API once for each of the 30x result status codes.
In the console window for the service, I could see what calls were made.

For example, if I do a POST to the /Redirect/initial/303 API in Firefox,
I see the following in the serrvice console.
This implies that Firefox followed the redirect and changed the method from POST to GET.

![](./server_console_post_303.png?raw=true)

For Net Framework and Net, I ran the client programs which do the same thing
(iterate over methods and 30x codes) and looked at what appeared in the console window for the service.

The swagger page:

![](./swagger_post_303.png?raw=true)

## Firefox (through swagger page)
<a name="h03" />

|Meth/Stat|300|301|302|303|307|308|
|---------|---|---|---|---|---|---|
|DELETE|DELETE|DELETE|DELETE|GET|DELETE|DELETE|
|GET|GET|GET|GET|GET|GET|GET|
|HEAD|HEAD *|HEAD *|HEAD|HEAD|HEAD|HEAD|
|OPTIONS|OPTIONS|OPTIONS|OPTIONS|GET|OPTIONS|OPTIONS|
|PATCH|PATCH|PATCH|PATCH|GET|PATCH|PATCH|
|POST|POST|GET|GET|GET|POST|POST|
|PUT|PUT|PUT|PUT|GET|PUT|PUT|

Firefox is unusual in that - for the HEAD method only - if the server
returns a 300 or 301, after following to the redirect URI the first time, it never uses the initial URI again.
If the initial URI is invoked again, Firefox ignores it and goes straight to the redirect URI.

## Chrome/Edge (through swagger page)
<a name="h04" />

|Meth/Stat|300|301|302|303|307|308|
|---------|---|---|---|---|---|---|
|DELETE|error|DELETE|DELETE|GET|DELETE|DELETE|
|GET|error|GET|GET|GET|GET|GET|
|HEAD|error|HEAD|HEAD|HEAD|HEAD|HEAD|
|OPTIONS|error|OPTIONS|OPTIONS|GET|OPTIONS|OPTIONS|
|PATCH|error|PATCH|PATCH|GET|PATCH|PATCH|
|POST|error|GET|GET|GET|POST|POST|
|PUT|error|PUT|PUT|GET|PUT|PUT|

Chrome and Edge exhibit identical behavior.
If the server returns a 300 response,
they show an "Error: Multiple Choices" in the swagger page
and do not follow the redirect.

## Net Framework 4.7.2
<a name="h05" />

|Meth/Stat|300|301|302|303|307|308|
|---------|---|---|---|---|---|---|
|DELETE|GET|DELETE|DELETE|GET|DELETE|none|
|GET|GET|GET|GET|GET|GET|none|
|HEAD|HEAD|HEAD|HEAD|HEAD|HEAD|none|
|OPTIONS|GET|OPTIONS|OPTIONS|GET|OPTIONS|none|
|PATCH|GET|PATCH|PATCH|GET|PATCH|none|
|POST|GET|GET|GET|GET|POST|none|
|PUT|GET|PUT|PUT|GET|PUT|none|

Net Framework does not follow the redirect on a 308 response.

## Net 5.0 (as of 2021-03-06)
<a name="h06" />

|Meth/Stat|300|301|302|303|307|308|
|---------|---|---|---|---|---|---|
|DELETE|DELETE|DELETE|DELETE|DELETE|DELETE|DELETE|
|GET|GET|GET|GET|GET|GET|GET|
|HEAD|HEAD|HEAD|HEAD|HEAD|HEAD|HEAD|
|OPTIONS|OPTIONS|OPTIONS|OPTIONS|OPTIONS|OPTIONS|OPTIONS|
|PATCH|PATCH|PATCH|PATCH|PATCH|PATCH|PATCH|
|POST|GET|GET|GET|GET|POST|POST|
|PUT|PUT|PUT|PUT|PUT|PUT|PUT|

## Donation
<a name="h07" />

If this project helped you, you can help me :) 

[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donate_SM.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=XE5JR3FR458ZE&currency_code=USD)
