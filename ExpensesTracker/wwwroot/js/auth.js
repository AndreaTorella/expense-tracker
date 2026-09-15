const tokenStorageKey = "expense-tracker-jwt";

export async function login(email, password) {
    const response = await fetch("/api/Auth/login", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({ email, password })
    });

    if (response.status === 401) {
        throw new Error("Credenziali non valide.");
    }

    if (!response.ok) {
        throw new Error("Non è stato possibile completare l'accesso.");
    }

    const { token } = await response.json();

    if (!token) {
        throw new Error("Il server non ha restituito un token di accesso valido.");
    }

    setToken(token);
}

export function getToken() {
    return sessionStorage.getItem(tokenStorageKey);
}

export function setToken(token) {
    sessionStorage.setItem(tokenStorageKey, token);
}

export function clearToken() {
    sessionStorage.removeItem(tokenStorageKey);
}

export function isAuthenticated() {
    return Boolean(getToken());
}

export function requireAuth() {
    if (isAuthenticated()) {
        return true;
    }

    redirectToLogin();
    return false;
}

export function logout() {
    clearToken();
    redirectToLogin();
}

export function redirectToLogin() {
    window.location.replace("/login.html");
}
