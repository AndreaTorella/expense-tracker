export function renderExpenses(expenses) {
    const tableBody = document.querySelector("#expenses-table-body");

    tableBody.innerHTML = "";

    for (const expense of expenses) {
        const row = document.createElement("tr");

        row.innerHTML = `
            <td>${formatDate(expense.date)}</td>
            <td>${expense.title}</td>
            <td>${expense.categoryName}</td>
            <td>${expense.paymentMethodName}</td>
            <td>${formatAmount(expense.amount)}</td>
        `;

        tableBody.appendChild(row);
    }
}

export function renderCategories(categories) {
    const categorySelect = document.querySelector("#category");

    for (const category of categories) {
        const option = document.createElement("option");

        option.value = category.id;
        option.textContent = category.name;

        categorySelect.appendChild(option);
    }
}

export function renderPaymentMethods(paymentMethods) {
    const paymentMethodSelect =
        document.querySelector("#payment-method");

    for (const paymentMethod of paymentMethods) {
        const option = document.createElement("option");

        option.value = paymentMethod.id;
        option.textContent = paymentMethod.name;

        paymentMethodSelect.appendChild(option);
    }
}

export function getExpenseFormData() {
    const titleInput = document.querySelector("#title");
    const amountInput = document.querySelector("#amount");
    const dateInput = document.querySelector("#date");
    const categorySelect = document.querySelector("#category");
    const paymentMethodSelect = document.querySelector("#payment-method");

    return {
        title: titleInput.value.trim(),
        amount: Number(amountInput.value),
        date: `${dateInput.value}T00:00:00`,
        categoryId: Number(categorySelect.value),
        paymentMethodId: Number(paymentMethodSelect.value)
    };
}

export function resetExpenseForm() {
    const form = document.querySelector("#expense-form");
    form.reset();
}

export function showLoading() {
    const loadingMessage = document.querySelector("#loading-message");
    loadingMessage.hidden = false;
}

export function hideLoading() {
    const loadingMessage = document.querySelector("#loading-message");
    loadingMessage.hidden = true;
}

export function showError(message) {
    const errorMessage = document.querySelector("#error-message");

    errorMessage.textContent = message;
    errorMessage.hidden = false;
}

function formatDate(dateValue) {
    const date = new Date(dateValue);

    return date.toLocaleDateString("it-IT");
}

function formatAmount(amount) {
    return amount.toLocaleString("it-IT", {
        style: "currency",
        currency: "EUR"
    });
}