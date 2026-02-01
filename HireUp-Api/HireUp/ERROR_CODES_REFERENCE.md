# HireUp API Error Codes Reference

This document provides a comprehensive reference of all error codes returned by the HireUp API, organized by endpoint and category.

---

## Error Response Format

All error responses follow this structure:

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.3.2",
  "title": "HTTP Status Title",
  "status": 400,
  "detail": "Detailed error description",
  "instance": null,
  "error": ["ErrorCode", "Error Description"]
}
```

### Response Fields

| Field | Type | Description |
|-------|------|-------------|
| `type` | string | RFC reference URL (RFC 7231 HTTP Status Codes) |
| `title` | string | HTTP status reason phrase (e.g., "Bad Request", "Unauthorized") |
| `status` | integer | HTTP status code (e.g., 400, 401, 404) |
| `detail` | string | Human-readable error description from the error code |
| `instance` | string/null | URI identifying specific occurrence of the problem |
| `error` | array[string] | HireUp custom field with [ErrorCode, ErrorDescription] |

---

## Error Codes by Category

### Authentication Errors (Status: 401)

These errors occur when authentication fails or JWT tokens are invalid.

| Error Code | Description | Endpoint(s) |
|-----------|-------------|-----------|
| `User.InvalidCredentials` | Email/password combination is incorrect | POST /auth |
| `User.EmailNotConfirmed` | User's email address has not been confirmed | POST /auth |
| `User.InvalidJwtToken` | Provided JWT token is invalid or expired | GET /user, PUT /user/me, POST /user/me/profile-picture |
| `User.InvalidRefreshToken` | Refresh token is invalid, expired, or already revoked | POST /auth/refresh, POST /auth/revoke-refresh-token |
| `User.InvalidCode` | Email confirmation code is invalid or expired | POST /auth/confirm-email |

### Validation & Request Errors (Status: 400, 422)

These errors occur when request data doesn't meet requirements.

| Error Code | Description | HTTP Status | Endpoint(s) |
|-----------|-------------|-----------|-----------|
| `Update.Failed` | Could not update user profile (database error) | 400 | PUT /user/me |
| `User.InvalidResetCode` | Password reset code is invalid or has expired | 400 | POST /auth/reset-password |
| `User.ResetPasswordFailed` | Unable to reset password, please try again | 400 | POST /auth/reset-password |
| Validation errors | Invalid input format, weak password, etc. | 422 | POST /auth/register, POST /auth/reset-password, PUT /user/me |

### Conflict Errors (Status: 409)

These errors indicate a resource already exists.

| Error Code | Description | Endpoint(s) |
|-----------|-------------|-----------|
| `User.DuplicatedEmail` | Email address is already registered by another user | POST /auth/register |
| `User.DuplicatedConfirmation` | Email address has already been confirmed | POST /auth/confirm-email |

### Not Found Errors (Status: 404)

These errors indicate the requested resource doesn't exist.

| Error Code | Description | Endpoint(s) |
|-----------|-------------|-----------|
| `User.NotFound` | User with specified ID not found | GET /user, GET /user/{userId}, PUT /user/me, POST /user/me/profile-picture, POST /auth/resend-confirmation-email |

### File Upload Errors (Status: 415)

These errors occur when file uploads have incorrect format.

| Error Code | Description | Endpoint(s) |
|-----------|-------------|-----------|
| (File validation) | Invalid file format or file size exceeds limit | POST /user/me/profile-picture |

---

## Errors by Endpoint

### POST /auth (Login)

**Possible Error Codes:**
- `User.InvalidCredentials` (401) - Email/password mismatch
- `User.EmailNotConfirmed` (401) - Email not confirmed yet
- Validation errors (400) - Missing/invalid email or password

**Example Error Response:**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.3.2",
  "title": "Unauthorized",
  "status": 401,
  "detail": "Invalid email/password",
  "error": ["User.InvalidCredentials", "Invalid email/password"]
}
```

---

### POST /auth/refresh (Refresh Token)

**Possible Error Codes:**
- `User.InvalidRefreshToken` (401) - Refresh token invalid/expired/revoked
- Validation errors (400) - Missing/invalid token fields

**Example Error Response:**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.3.2",
  "title": "Unauthorized",
  "status": 401,
  "detail": "Invalid refresh token",
  "error": ["User.InvalidRefreshToken", "Invalid refresh token"]
}
```

---

### POST /auth/revoke-refresh-token (Revoke Token)

**Possible Error Codes:**
- `User.InvalidRefreshToken` (401) - Token invalid/expired/already revoked
- Validation errors (400) - Missing/invalid token fields

---

### POST /auth/register (Register)

**Possible Error Codes:**
- `User.DuplicatedEmail` (409) - Email already registered
- Validation errors (422) - Password too weak, invalid email format, missing fields
- Request errors (400) - Invalid request format

**Example Error Response (Duplicate Email):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.3.2",
  "title": "Conflict",
  "status": 409,
  "detail": "Another user with the same email is already exists",
  "error": ["User.DuplicatedEmail", "Another user with the same email is already exists"]
}
```

**Example Error Response (Weak Password):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.3.2",
  "title": "Unprocessable Entity",
  "status": 422,
  "detail": "Password must be at least 8 characters long",
  "error": ["Validation.PasswordTooShort", "Password must be at least 8 characters long"]
}
```

---

### POST /auth/confirm-email (Confirm Email)

**Possible Error Codes:**
- `User.InvalidCode` (401) - Confirmation code invalid/expired
- `User.NotFound` (404) - User ID not found
- `User.DuplicatedConfirmation` (400) - Email already confirmed
- Request errors (400) - Invalid request format

**Example Error Response:**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.3.2",
  "title": "Unauthorized",
  "status": 401,
  "detail": "Invalid Code",
  "error": ["User.InvalidCode", "Invalid Code"]
}
```

---

### POST /auth/resend-confirmation-email (Resend Confirmation)

**Possible Error Codes:**
- `User.NotFound` (404) - User with email not found
- Request errors (400) - Invalid email or request format

---

### POST /auth/forget-password (Request Password Reset)

**Possible Error Codes:**
- `User.NotFound` (404) - User with email not found
- Request errors (400) - Invalid email or request format

---

### POST /auth/reset-password (Reset Password)

**Possible Error Codes:**
- `User.InvalidResetCode` (400) - Reset code invalid or expired
- `User.ResetPasswordFailed` (400) - Database error during reset
- `User.NotFound` (404) - User with email not found
- Validation errors (422) - Password doesn't meet requirements

**Example Error Response:**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.3.2",
  "title": "Bad Request",
  "status": 400,
  "detail": "The code is invalid or has expired. Please try again or request a new one.",
  "error": ["User.InvalidResetCode", "The code is invalid or has expired. Please try again or request a new one."]
}
```

---

### GET /user (Get My Profile)

**Possible Error Codes:**
- `User.InvalidJwtToken` (401) - JWT token invalid/missing
- `User.NotFound` (404) - User profile not found

**Example Error Response:**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.3.2",
  "title": "Unauthorized",
  "status": 401,
  "detail": "The provided JWT Token is invalid",
  "error": ["User.InvalidJwtToken", "The provided JWT Token is invalid"]
}
```

---

### GET /user/{userId} (Get Public Profile)

**Possible Error Codes:**
- `User.NotFound` (404) - User not found

---

### PUT /user/me (Update Profile)

**Possible Error Codes:**
- `User.InvalidJwtToken` (401) - JWT token invalid/missing
- `Update.Failed` (400) - Database error during update
- `User.NotFound` (404) - User not found
- Validation errors (422) - Invalid field values

**Example Error Response:**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.3.2",
  "title": "Bad Request",
  "status": 400,
  "detail": "Could not update user profile.",
  "error": ["Update.Failed", "Could not update user profile."]
}
```

---

### POST /user/me/profile-picture (Upload Profile Picture)

**Possible Error Codes:**
- `User.InvalidJwtToken` (401) - JWT token invalid/missing
- `User.NotFound` (404) - User not found
- File validation errors (400) - Invalid file format or size exceeds limit
- File type errors (415) - File is not a valid image format

---

## HTTP Status Codes Reference

| Status Code | Title | Meaning | Common Causes |
|------------|-------|---------|--------------|
| 200 | OK | Successful request | Login, token refresh, fetch profile |
| 204 | No Content | Successful request, no response body | Profile update |
| 400 | Bad Request | Invalid request or server error | Validation failed, database error, expired code |
| 401 | Unauthorized | Authentication failed | Invalid credentials, invalid JWT, invalid code |
| 404 | Not Found | Resource not found | User not found |
| 409 | Conflict | Resource already exists | Duplicate email |
| 415 | Unsupported Media Type | Invalid file format | Invalid image file type |
| 422 | Unprocessable Entity | Validation error | Weak password, invalid format |

---

## Error Handling Best Practices

### For Frontend Developers

1. **Always check the `error` array** for the error code, not just the HTTP status
   ```javascript
   const errorCode = response.error[0];  // e.g., "User.InvalidCredentials"
   const errorDescription = response.error[1];  // User-friendly message
   ```

2. **Different handling for different error codes:**
   ```javascript
   switch(errorCode) {
     case 'User.InvalidCredentials':
       // Show "Email or password is incorrect"
       break;
     case 'User.DuplicatedEmail':
       // Show "Email is already registered"
       break;
     case 'User.InvalidJwtToken':
       // Redirect to login
       break;
   }
   ```

3. **Use the `detail` field for user-facing error messages**
   - Already localized if available
   - Safe to display to end users
   - Contains the description from the error code

4. **Log the full error response** for debugging:
   ```javascript
   console.error('API Error:', {
     status: response.status,
     errorCode: response.error[0],
     description: response.detail,
     fullResponse: response
   });
   ```

---

## Password Requirements

When working with password-related endpoints (`POST /auth/register`, `POST /auth/reset-password`), passwords must meet these requirements:

- **Minimum length:** 8 characters
- **Recommended:** Mix of uppercase, lowercase, numbers, and special characters
- **Validation:** Enforced by the API

When these requirements aren't met, you'll receive a 422 Unprocessable Entity status with validation error details.

---

## Token Expiration

- **Access Token (JWT):** Expires in 1 hour (3600 seconds)
- **Refresh Token:** Long-lived, no specific expiration in documentation
- **Email Confirmation Code:** Typically valid for 24-48 hours (varies by configuration)
- **Password Reset Code:** Typically valid for 24 hours (varies by configuration)

When tokens expire, you'll receive a `User.InvalidJwtToken` (401) or `User.InvalidRefreshToken` (401) error.

---

## Email Confirmation Requirement

- Users **must** confirm their email before logging in
- Confirmation code is sent automatically upon registration
- If not received, use `POST /auth/resend-confirmation-email`
- Attempting to login without confirmation returns `User.EmailNotConfirmed` (401)

---

## Additional Resources

- See `API_DOCUMENTATION.md` for workflows and examples
- See controller XML documentation in Swagger UI for endpoint-specific details
- Review error source in `UserErrors.cs` for all available error codes
