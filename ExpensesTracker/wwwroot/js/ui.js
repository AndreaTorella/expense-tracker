let loadingRequests = 0;

export function renderTransactions(transactions) {
    const tableBody = document.querySelector("#transactions-table-body");
    tableBody.innerHTML = "";

    for (const transaction of transactions) {
        const row = document.createElement("tr");
        const transactionType = transaction.transactionType || "Expense";
        const typeLabel = transactionType === "Income" ? "Entrata" : "Uscita";

        row.innerHTML = `
            <td>${formatDate(transaction.date)}</td>
            <td><div class="transaction-title"></div><div class="transaction-meta">ID ${transaction.id}</div></td>
            <td><span class="transaction-type transaction-type-${transactionType.toLowerCase()}">${typeLabel}</span></td>
            <td><span class="category-pill"></span></td>
            <td><span class="payment-pill"></span></td>
            <td class="text-end transaction-amount transaction-amount-${transactionType.toLowerCase()}">${formatAmount(transaction.amount, transactionType)}</td>
            <td><div class="row-actions">
                <button type="button" class="btn btn-outline-secondary btn-sm edit-transaction-button" data-transaction-id="${transaction.id}"><i class="bi bi-pencil-square" aria-hidden="true"></i><span class="visually-hidden">Modifica movimento</span></button>
                <button type="button" class="btn btn-outline-danger btn-sm delete-transaction-button" data-transaction-id="${transaction.id}"><i class="bi bi-trash3" aria-hidden="true"></i><span class="visually-hidden">Elimina movimento</span></button>
            </div></td>`;

        row.querySelector(".transaction-title").textContent = transaction.title;
        row.querySelector(".category-pill").textContent = transaction.categoryName;
        row.querySelector(".payment-pill").textContent = transaction.paymentMethodName;
        tableBody.appendChild(row);
    }
}

export function renderCategories(categories, transactionType, selectedCategoryId = "") {
    appendSelectOptions("#category", categories, "Seleziona una categoria", transactionType, selectedCategoryId);
}

export function renderFilterCategories(categories, transactionType, selectedCategoryId = "") {
    appendSelectOptions("#filter-category", categories, "Tutte", transactionType, selectedCategoryId);
}

export function renderPaymentMethods(paymentMethods) {
    appendSelectOptions("#payment-method", paymentMethods, "Seleziona un metodo");
}

export function renderFilterPaymentMethods(paymentMethods) {
    appendSelectOptions("#filter-payment-method", paymentMethods, "Tutti");
}

export function getTransactionFilters() {
    return {
        Search: document.querySelector("#filter-search").value.trim(),
        FromDate: document.querySelector("#filter-from-date").value,
        ToDate: document.querySelector("#filter-to-date").value,
        TransactionType: document.querySelector("#filter-transaction-type").value,
        CategoryId: document.querySelector("#filter-category").value,
        PaymentMethodId: document.querySelector("#filter-payment-method").value
    };
}

export function getTransactionSorting() {
    const [transactionSortBy, sortDirection] = document.querySelector("#transactions-sort").value.split(":");
    return { TransactionSortBy: transactionSortBy, SortDirection: sortDirection };
}

export function getTransactionsPageSize() {
    return Number(document.querySelector("#transactions-page-size").value);
}

export function updateTransactionsSummary(pagedTransactions) {
    const summary = document.querySelector("#transactions-summary");
    const { pageNumber, pageSize, totalItems } = pagedTransactions;
    if (totalItems === 0) {
        summary.textContent = "Nessun movimento trovato";
        return;
    }
    const firstItem = (pageNumber - 1) * pageSize + 1;
    const lastItem = Math.min(pageNumber * pageSize, totalItems);
    summary.textContent = `Visualizzati ${firstItem}-${lastItem} di ${totalItems} movimenti`;
}

export function renderPagination(pagedTransactions) {
    const pagination = document.querySelector("#transactions-pagination");
    const { pageNumber, totalPages } = pagedTransactions;
    pagination.innerHTML = "";
    if (totalPages <= 1) return;
    appendPageButton(pagination, pageNumber - 1, "Precedente", pageNumber === 1);
    for (const page of getVisiblePages(pageNumber, totalPages)) {
        if (page === null) appendPaginationEllipsis(pagination);
        else appendPageButton(pagination, page, `Pagina ${page}`, false, page === pageNumber);
    }
    appendPageButton(pagination, pageNumber + 1, "Successiva", pageNumber === totalPages);
}

export function getTransactionFormData() {
    return {
        title: document.querySelector("#title").value.trim(),
        amount: Number(document.querySelector("#amount").value),
        date: `${document.querySelector("#date").value}T00:00:00`,
        transactionType: document.querySelector("#transaction-type").value,
        categoryId: Number(document.querySelector("#category").value),
        paymentMethodId: Number(document.querySelector("#payment-method").value)
    };
}

export function setDefaultTransactionDate() {
    const dateInput = document.querySelector("#date");
    if (!dateInput.value) dateInput.value = new Date().toISOString().split("T")[0];
}

export function resetTransactionForm() {
    document.querySelector("#transaction-form").reset();
}

export function hideTransactionModal() {
    const modalElement = document.querySelector("#transaction-modal");
    const modal = modalElement && window.bootstrap && window.bootstrap.Modal.getInstance(modalElement);
    modal?.hide();
}

export function showLoading() {
    const loadingOverlay = getLoadingOverlay();
    loadingRequests += 1;
    loadingOverlay.hidden = false;
    document.body.classList.add("app-is-loading");
    document.body.setAttribute("aria-busy", "true");
}

export function hideLoading() {
    const loadingOverlay = getLoadingOverlay();
    loadingRequests = Math.max(loadingRequests - 1, 0);
    if (loadingRequests > 0) return;
    loadingOverlay.hidden = true;
    document.body.classList.remove("app-is-loading");
    document.body.removeAttribute("aria-busy");
}

export function showError(message) {
    const errorMessage = document.querySelector("#error-message");
    errorMessage.textContent = message;
    errorMessage.hidden = false;
    showToast(message, "error");
}

export function showToast(message, type = "success") {
    const toastContainer = document.querySelector("#toast-container");
    if (!toastContainer || !window.bootstrap) return;
    const isError = type === "error";
    const toastElement = document.createElement("div");
    toastElement.className = `toast app-toast ${isError ? "app-toast-error" : "app-toast-success"}`;
    toastElement.setAttribute("role", "status");
    toastElement.setAttribute("aria-live", isError ? "assertive" : "polite");
    toastElement.setAttribute("aria-atomic", "true");
    toastElement.innerHTML = `<div class="toast-body"><i class="bi ${isError ? "bi-exclamation-circle-fill" : "bi-check-circle-fill"}" aria-hidden="true"></i><span></span><button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Chiudi"></button></div>`;
    toastElement.querySelector("span").textContent = message;
    toastContainer.appendChild(toastElement);
    toastElement.addEventListener("hidden.bs.toast", () => toastElement.remove());
    window.bootstrap.Toast.getOrCreateInstance(toastElement, { delay: 5000 }).show();
}

export function confirmTransactionDeletion() {
    const modalElement = document.querySelector("#delete-transaction-modal");
    const confirmButton = document.querySelector("#confirm-delete-transaction-button");
    if (!modalElement || !confirmButton || !window.bootstrap) return Promise.resolve(false);
    return new Promise((resolve) => {
        const modal = window.bootstrap.Modal.getOrCreateInstance(modalElement);
        let confirmed = false;
        const handleConfirmation = () => { confirmed = true; modal.hide(); };
        const handleModalHidden = () => { confirmButton.removeEventListener("click", handleConfirmation); resolve(confirmed); };
        confirmButton.addEventListener("click", handleConfirmation);
        modalElement.addEventListener("hidden.bs.modal", handleModalHidden, { once: true });
        modal.show();
    });
}

function appendSelectOptions(selector, options, placeholder, transactionType, selectedValue = "") {
    const select = document.querySelector(selector);
    select.innerHTML = "";
    const placeholderOption = new Option(placeholder, "");
    select.appendChild(placeholderOption);
    for (const optionData of options) {
        if (transactionType && optionData.transactionType !== undefined && optionData.transactionType !== transactionType) continue;
        const option = new Option(optionData.name, optionData.id);
        option.selected = String(optionData.id) === String(selectedValue);
        select.appendChild(option);
    }
}

function formatDate(dateValue) { return new Date(dateValue).toLocaleDateString("it-IT"); }
function formatAmount(amount, transactionType) {
    const sign = transactionType === "Income" ? "+" : "-";
    return `${sign} ${Number(amount).toLocaleString("it-IT", { style: "currency", currency: "EUR" })}`;
}
function getVisiblePages(currentPage, totalPages) {
    const pages = new Set([1, totalPages]);
    for (let page = currentPage - 1; page <= currentPage + 1; page += 1) if (page > 1 && page < totalPages) pages.add(page);
    const visiblePages = []; let previousPage = 0;
    for (const page of [...pages].sort((a, b) => a - b)) { if (page - previousPage > 1) visiblePages.push(null); visiblePages.push(page); previousPage = page; }
    return visiblePages;
}
function appendPageButton(pagination, pageNumber, label, disabled, isActive = false) {
    const item = document.createElement("li"); item.className = `page-item${isActive ? " active" : ""}${disabled ? " disabled" : ""}`;
    const button = document.createElement("button"); button.type = "button"; button.className = "page-link"; button.dataset.pageNumber = pageNumber; button.textContent = isActive ? String(pageNumber) : label; button.disabled = disabled; button.setAttribute("aria-label", label);
    if (isActive) button.setAttribute("aria-current", "page"); item.appendChild(button); pagination.appendChild(item);
}
function appendPaginationEllipsis(pagination) { const item = document.createElement("li"); item.className = "page-item disabled"; item.setAttribute("aria-hidden", "true"); item.innerHTML = '<span class="page-link">…</span>'; pagination.appendChild(item); }
function getLoadingOverlay() {
    let loadingOverlay = document.querySelector("#app-loading-overlay");
    if (loadingOverlay) return loadingOverlay;
    loadingOverlay = document.createElement("div"); loadingOverlay.id = "app-loading-overlay"; loadingOverlay.className = "loading-overlay"; loadingOverlay.setAttribute("role", "status"); loadingOverlay.setAttribute("aria-live", "polite"); loadingOverlay.hidden = true;
    loadingOverlay.innerHTML = `<div class="loading-spinner" aria-hidden="true">${"<span></span>".repeat(12)}</div><span class="visually-hidden">Operazione in corso...</span>`;
    document.body.appendChild(loadingOverlay); return loadingOverlay;
}
