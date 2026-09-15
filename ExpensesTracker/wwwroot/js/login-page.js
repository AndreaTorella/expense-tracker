import { isAuthenticated, login } from "./auth.js";

const form = document.querySelector("#login-form");
const errorMessage = document.querySelector("#login-error-message");
const submitButton = document.querySelector("#login-button");

if (isAuthenticated()) {
    window.location.replace("/index.html");
} else {
    form.addEventListener("submit", handleLoginSubmit);
}

async function handleLoginSubmit(event) {
    event.preventDefault();
    hideError();

    const email = form.elements.email.value.trim();
    const password = form.elements.password.value;

    submitButton.disabled = true;
    submitButton.innerHTML = '<span class="spinner-border spinner-border-sm" aria-hidden="true"></span> Accesso in corso';

    try {
        await login(email, password);
        window.location.replace("/index.html");
    } catch (error) {
        console.error(error);
        showError(error.message);
    } finally {
        submitButton.disabled = false;
        submitButton.innerHTML = '<i class="bi bi-box-arrow-in-right" aria-hidden="true"></i> Accedi';
    }
}

function showError(message) {
    errorMessage.textContent = message;
    errorMessage.hidden = false;
}

function hideError() {
    errorMessage.hidden = true;
    errorMessage.textContent = "";
}
