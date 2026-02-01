# Quick Reference: HireUp API Swagger Documentation

## ?? TL;DR - What Changed

Your original concern was **correct**: The error response format was incorrectly documented. 

? **Fixed:** All endpoints now show the correct error format with the `error` array:

```json
{
  "error": ["User.InvalidCredentials", "Invalid email/password"]
}
```

This matches your `ResultExtensions.cs` implementation.

---

## ?? Access Swagger UI

**URL:** `http://localhost:5000/swagger`

---

## ?? Documentation Files Created

| File | Purpose |
|------|---------|
| `API_DOCUMENTATION.md` | Complete API reference with workflows |
| `ERROR_CODES_REFERENCE.md` | All error codes with examples |
| `SWAGGER_DOCUMENTATION_UPDATE.md` | What was fixed |
| `IMPLEMENTATION_SUMMARY.md` | Complete implementation details |

---

## ?? Error Code Examples

### Most Common
- `User.InvalidCredentials` (401) - Wrong password
- `User.DuplicatedEmail` (409) - Email taken
- `User.InvalidJwtToken` (401) - Bad token
- `User.NotFound` (404) - User doesn't exist

### Registration
- `User.DuplicatedEmail` (409) - Email already registered
- Validation errors (422) - Password too weak

### Login
- `User.InvalidCredentials` (401) - Wrong email/password
- `User.EmailNotConfirmed` (401) - Email not verified

### Password Reset
- `User.InvalidResetCode` (400) - Code invalid/expired
- `User.NotFound` (404) - Email not registered

---

## ?? All Endpoints (12 Total)

### Auth (8 endpoints)
```
POST   /auth                          Login
POST   /auth/refresh                  Refresh token
POST   /auth/revoke-refresh-token     Logout
POST   /auth/register                 Register
POST   /auth/confirm-email            Confirm email
POST   /auth/resend-confirmation-email Resend code
POST   /auth/forget-password          Password reset request
POST   /auth/reset-password           Password reset
```

### User (4 endpoints)
```
GET    /user                          Get my profile (JWT required)
GET    /user/{userId}                 Get public profile
PUT    /user/me                       Update profile (JWT required)
POST   /user/me/profile-picture       Upload picture (JWT required)
```

---

## ??? Authentication

### How to Authenticate in Swagger

1. Call `POST /auth` with email/password
2. Copy the `token` value from response
3. Click "Authorize" button (top right)
4. Enter: `Bearer eyJhbGciOiJIUzI1NiIs...`
5. Protected endpoints now work

### Token Info
- **Access Token:** Expires in 1 hour
- **Refresh Token:** Long-lived
- **Use:** Include in `Authorization: Bearer <token>` header

---

## ?? Status Codes

| Code | Meaning | Common Use |
|------|---------|-----------|
| 200 | Success | Login, get profile |
| 204 | Success (no body) | Update profile |
| 400 | Bad request | Invalid input |
| 401 | Unauthorized | Invalid/missing JWT |
| 404 | Not found | User doesn't exist |
| 409 | Conflict | Email taken |
| 415 | Wrong file type | Invalid image upload |
| 422 | Validation error | Weak password |

---

## ?? Error Response Format

Every error includes the error code and description:

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.3.2",
  "title": "Unauthorized",
  "status": 401,
  "detail": "Invalid email/password",
  "error": ["User.InvalidCredentials", "Invalid email/password"]
}
```

Use `error[0]` for the error code to determine how to handle it.

---

## ?? Common Workflows

### User Registration ? Login ? Access Profile

```
1. POST /auth/register
   ? (Confirmation email sent)
2. POST /auth/confirm-email (with code from email)
   ? (Email confirmed)
3. POST /auth
   ? (Get JWT tokens)
4. GET /user (with JWT)
   ? (Get authenticated user's profile)
```

### Password Reset

```
1. POST /auth/forget-password
   ? (Reset code sent to email)
2. POST /auth/reset-password (with code)
   ? (Password changed)
3. POST /auth (login with new password)
```

### Token Refresh

```
1. POST /auth (get tokens)
   ? (Access token expires after 1 hour)
2. POST /auth/refresh (with refresh token)
   ? (Get new tokens)
```

---

## ? Build Status

? **Successful** - All changes compile correctly

---

## ?? Ready to Use

Everything is documented and ready. Just run the application and access `/swagger` to see:
- All endpoints with full descriptions
- Request/response examples
- All possible error codes
- Sample error responses
- Authentication requirements

---

## ?? Need More Details?

- **Full workflows:** See `API_DOCUMENTATION.md`
- **All error codes:** See `ERROR_CODES_REFERENCE.md`
- **What changed:** See `SWAGGER_DOCUMENTATION_UPDATE.md`
- **Implementation details:** See `IMPLEMENTATION_SUMMARY.md`

---

## ? Summary

? Error format corrected  
? 12 endpoints fully documented  
? Sample responses provided  
? Error codes mapped  
? JWT authentication configured  
? All documentation files created  
? Build successful  

**Your API is now fully documented!** ??
