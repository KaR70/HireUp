# HireUp API Documentation

## Overview
HireUp is a .NET 8 REST API for authentication and user management. All endpoints return responses in JSON format.

---

## Error Response Format

### Standard Error Response Structure
All error responses follow the RFC 7807 Problem Details specification with custom HireUp extensions:

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.3.2",
  "title": "Unauthorized",
  "status": 401,
  "detail": "Invalid email/password",
  "instance": null,
  "error": ["ErrorCode", "Error Description"]
}
```

### Error Response Properties

| Property | Type | Description |
|----------|------|-------------|
| `type` | string | RFC reference URL (always present) |
| `title` | string | HTTP status reason phrase |
| `status` | integer | HTTP status code |
| `detail` | string | Detailed error description |
| `instance` | string/null | URI reference to specific occurrence |
| `error` | array[string] | Custom array: [ErrorCode, ErrorDescription] |

### Common Error Codes and Status Codes

#### Authentication Errors

| Error Code | Description | Status Code |
|-----------|-------------|------------|
| `User.InvalidCredentials` | Invalid email/password combination | 401 |
| `User.EmailNotConfirmed` | Email has not been confirmed by user | 401 |
| `User.InvalidJwtToken` | The provided JWT token is invalid or expired | 401 |
| `User.InvalidRefreshToken` | Refresh token is invalid or expired | 401 |

#### Validation/Request Errors

| Error Code | Description | Status Code |
|-----------|-------------|------------|
| `User.DuplicatedEmail` | Email already registered by another user | 409 |
| `User.InvalidCode` | Invalid email confirmation code | 401 |
| `User.InvalidResetCode` | Password reset code is invalid or expired | 400 |
| `Update.Failed` | Could not update user profile | 400 |

#### Not Found Errors

| Error Code | Description | Status Code |
|-----------|-------------|------------|
| `User.NotFound` | User associated with token not found | 404 |

---

## Authentication

### JWT Bearer Token
Most endpoints require authentication using JWT Bearer tokens obtained from the login endpoint.

**How to authenticate in Swagger UI:**
1. Call the `/auth` endpoint to get a JWT token
2. Click the "Authorize" button in Swagger UI
3. Enter: `Bearer <your-jwt-token>`
4. Click "Authorize"

**Token Expiration:**
- Access tokens expire in 1 hour
- Use `/auth/refresh` endpoint to get new tokens before expiration
- Refresh tokens are long-lived and stored securely

---

## Endpoints Overview

### Authentication Controller (`/auth`)

| Method | Endpoint | Auth Required | Description |
|--------|----------|---------------|-------------|
| POST | `/auth` | No | Login with email/password |
| POST | `/auth/refresh` | No | Refresh access token |
| POST | `/auth/revoke-refresh-token` | No | Revoke refresh token |
| POST | `/auth/register` | No | Register new user |
| POST | `/auth/confirm-email` | No | Confirm email with code |
| POST | `/auth/resend-confirmation-email` | No | Resend confirmation email |
| POST | `/auth/forget-password` | No | Request password reset code |
| POST | `/auth/reset-password` | No | Reset password with code |

### User Controller (`/user`)

| Method | Endpoint | Auth Required | Description |
|--------|----------|---------------|-------------|
| GET | `/user` | Yes | Get authenticated user's profile |
| GET | `/user/{userId}` | No | Get public profile of any user |
| PUT | `/user/me` | Yes | Update authenticated user's profile |
| POST | `/user/me/profile-picture` | Yes | Upload/update profile picture |

---

## Workflow Examples

### 1. User Registration Flow

```
POST /auth/register
  ? (User created, confirmation email sent)
POST /auth/confirm-email (with code from email)
  ? (Email confirmed)
POST /auth (login with credentials)
  ? (Returns JWT tokens)
```

### 2. Password Reset Flow

```
POST /auth/forget-password (email)
  ? (Reset code sent to email)
POST /auth/reset-password (email, code, newPassword)
  ? (Password updated)
POST /auth (login with new password)
  ? (Returns JWT tokens)
```

### 3. Token Refresh Flow

```
POST /auth (login)
  ? (Returns accessToken + refreshToken)
[Access token expires after 1 hour]
POST /auth/refresh (with expired token + refreshToken)
  ? (Returns new tokens)
```

### 4. User Profile Access Flow

```
POST /auth (login)
  ? (Returns JWT token)
GET /user (with JWT in Authorization header)
  ? (Returns authenticated user's full profile)
GET /user/{userId} (no auth needed)
  ? (Returns public profile of specified user)
PUT /user/me (with JWT)
  ? (Updates profile, returns 204 No Content)
POST /user/me/profile-picture (with JWT + multipart/form-data)
  ? (Returns { profilePictureUrl: "..." })
```

---

## Request/Response Examples

### Login Request
```http
POST /auth HTTP/1.1
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "SecurePassword123!"
}
```

### Login Response (200 OK)
```json
{
  "id": "user-123",
  "email": "user@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresIn": 3600,
  "refreshToken": "refresh-token-abc123...",
  "refreshTokenExpiration": "2024-12-31T23:59:59Z"
}
```

### Error Response (401 Unauthorized)
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.3.2",
  "title": "Unauthorized",
  "status": 401,
  "detail": "Invalid email/password",
  "instance": null,
  "error": ["User.InvalidCredentials", "Invalid email/password"]
}
```

### Get User Profile Response (200 OK)
```json
{
  "id": "user-123",
  "email": "user@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "profilePictureUrl": "https://api.example.com/images/profile/user-123.jpg",
  "bio": "Software developer",
  "skills": ["C#", ".NET", "SQL"],
  "accessibilityNeeds": ["Keyboard navigation", "High contrast"],
  "createdAt": "2024-01-15T10:30:00Z"
}
```

### Update Profile Picture Response (200 OK)
```json
{
  "profilePictureUrl": "https://api.example.com/images/profile/user-123.jpg"
}
```

---

## Status Codes Reference

| Code | Meaning | Usage |
|------|---------|-------|
| 200 | OK | Successful request with response body |
| 204 | No Content | Successful request without response body (e.g., PUT) |
| 400 | Bad Request | Invalid input, validation failed, or server error |
| 401 | Unauthorized | Invalid/missing JWT or invalid credentials |
| 404 | Not Found | Resource not found |
| 409 | Conflict | Email already registered |
| 415 | Unsupported Media Type | Invalid file format for uploads |
| 422 | Unprocessable Entity | Validation error in request data |

---

## Security Notes

1. **JWT Tokens**: Always include the Bearer token in the `Authorization` header
2. **HTTPS**: All API calls should use HTTPS in production
3. **Token Storage**: Store refresh tokens securely (httpOnly cookies recommended)
4. **Email Confirmation**: Users must confirm their email before login
5. **Password Requirements**: Minimum 8 characters, enforced by the API

---

## Pagination and Filtering

Currently, the HireUp API does not implement pagination or filtering on list endpoints. All endpoints return complete results.

---

## Rate Limiting

Rate limiting is not currently implemented but may be added in future versions.

---

## API Version

Current Version: **v1**

For future versioning, use the URL pattern: `/api/v{version}/endpoint`
