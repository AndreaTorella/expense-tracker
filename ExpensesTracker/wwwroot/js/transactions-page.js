import { loadTransactions, loadCategories, loadPaymentMethods, saveTransaction, loadTransactionById, modifyTransaction, removeTransaction } from "./transactions.js";
import { logout, requireAuth } from "./auth.js";
import { renderTransactions, renderCategories, renderFilterCategories, renderPaymentMethods, renderFilterPaymentMethods, showLoading, hideLoading, showError, getTransactionFormData, resetTransactionForm, setDefaultTransactionDate, hideTransactionModal, showToast, confirmTransactionDeletion, getTransactionFilters, getTransactionSorting, getTransactionsPageSize, updateTransactionsSummary, renderPagination } from "./ui.js";

let editingTransactionId = null;
let currentPageNumber = 1;
let categories = [];

async function initializePage() {
    try {
        showLoading();
        const [pagedTransactions, loadedCategories, paymentMethods] = await Promise.all([
            loadTransactions(getTransactionsQuery()), loadCategories(), loadPaymentMethods()
        ]);
        categories = loadedCategories;
        renderPagedTransactions(pagedTransactions);
        refreshCategoryOptions();
        renderPaymentMethods(paymentMethods);
        renderFilterPaymentMethods(paymentMethods);
        setDefaultTransactionDate();

        document.querySelector("#transaction-form").addEventListener("submit", handleTransactionFormSubmit);
        document.querySelector("#transactions-table-body").addEventListener("click", handleTransactionsTableClick);
        document.querySelector("#filters-form").addEventListener("submit", handleFiltersSubmit);
        document.querySelector("#reset-filters-button").addEventListener("click", handleFiltersReset);
        document.querySelector("#transactions-sort").addEventListener("change", () => refreshWithLoading(1, "Non è stato possibile ordinare i movimenti."));
        document.querySelector("#transactions-page-size").addEventListener("change", () => refreshWithLoading(1, "Non è stato possibile aggiornare i movimenti per pagina."));
        document.querySelector("#transactions-pagination").addEventListener("click", handlePaginationClick);
        document.querySelector("#transaction-type").addEventListener("change", () => refreshCategoryOptions());
        document.querySelector("#filter-transaction-type").addEventListener("change", () => refreshCategoryOptions());

        const newTransactionButton = document.querySelector("#new-transaction-button");
        newTransactionButton.addEventListener("click", prepareNewTransaction);
        if (window.location.hash === "#new-transaction") newTransactionButton.click();
    } catch (error) {
        console.error(error);
        showError("Non è stato possibile caricare i dati.");
    } finally { hideLoading(); }
}

function prepareNewTransaction() {
    editingTransactionId = null;
    resetTransactionForm();
    document.querySelector("#transaction-type").value = "Expense";
    refreshCategoryOptions();
    setDefaultTransactionDate();
    document.querySelector("#transaction-modal-title").textContent = "Nuovo movimento";
}

async function handleTransactionFormSubmit(event) {
    event.preventDefault(); showLoading();
    try {
        const transaction = getTransactionFormData();
        const isNewTransaction = editingTransactionId === null;
        if (isNewTransaction) await saveTransaction(transaction);
        else await modifyTransaction(editingTransactionId, transaction);
        editingTransactionId = null; resetTransactionForm(); setDefaultTransactionDate(); hideTransactionModal();
        await refreshTransactions();
        showToast(isNewTransaction ? "Movimento aggiunto correttamente" : "Movimento modificato");
    } catch (error) { console.error(error); showError("Non è stato possibile salvare il movimento."); }
    finally { hideLoading(); }
}

async function handleTransactionsTableClick(event) {
    const updateButton = event.target.closest(".edit-transaction-button");
    const deleteButton = event.target.closest(".delete-transaction-button");
    if (updateButton) await handleEditTransaction(updateButton);
    if (deleteButton) await handleDeleteTransaction(deleteButton);
}

async function handleEditTransaction(updateButton) {
    const transactionId = Number(updateButton.dataset.transactionId);
    try {
        showLoading();
        const transaction = await loadTransactionById(transactionId);
        editingTransactionId = transactionId;
        populateTransactionForm(transaction);
        document.querySelector("#transaction-modal-title").textContent = "Modifica movimento";
        bootstrap.Modal.getOrCreateInstance(document.querySelector("#transaction-modal")).show();
    } catch (error) { console.error(error); showError("Non è stato possibile caricare il movimento da modificare."); }
    finally { hideLoading(); }
}

async function handleDeleteTransaction(deleteButton) {
    const transactionId = Number(deleteButton.dataset.transactionId);
    if (!await confirmTransactionDeletion()) return;
    try { showLoading(); await removeTransaction(transactionId); await refreshTransactions(); showToast("Movimento eliminato"); }
    catch (error) { console.error(error); showError("Non è stato possibile eliminare il movimento."); }
    finally { hideLoading(); }
}

async function handleFiltersSubmit(event) {
    event.preventDefault();
    const filters = getTransactionFilters();
    if (filters.FromDate && filters.ToDate && filters.FromDate > filters.ToDate) { showError("La data iniziale non può essere successiva alla data finale."); return; }
    await refreshWithLoading(1, "Non è stato possibile applicare i filtri ai movimenti.", filters);
}

async function handleFiltersReset() {
    document.querySelector("#filters-form").reset(); refreshCategoryOptions();
    await refreshWithLoading(1, "Non è stato possibile ripristinare l'elenco dei movimenti.", {});
}

async function handlePaginationClick(event) {
    const pageButton = event.target.closest("[data-page-number]");
    if (!pageButton || pageButton.disabled) return;
    await refreshWithLoading(Number(pageButton.dataset.pageNumber), "Non è stato possibile caricare la pagina richiesta.");
}

async function refreshWithLoading(pageNumber, errorMessage, filters = getTransactionFilters()) {
    try { showLoading(); await refreshTransactions(pageNumber, filters); }
    catch (error) { console.error(error); showError(errorMessage); }
    finally { hideLoading(); }
}

async function refreshTransactions(pageNumber = currentPageNumber, filters = getTransactionFilters()) {
    const pagedTransactions = await loadTransactions(getTransactionsQuery(pageNumber, filters));
    if (pagedTransactions.items.length === 0 && pagedTransactions.totalItems > 0 && pageNumber > pagedTransactions.totalPages) return await refreshTransactions(pagedTransactions.totalPages, filters);
    renderPagedTransactions(pagedTransactions);
}

function getTransactionsQuery(pageNumber = currentPageNumber, filters = getTransactionFilters()) {
    return { ...filters, ...getTransactionSorting(), PageNumber: pageNumber, PageSize: getTransactionsPageSize() };
}

function renderPagedTransactions(pagedTransactions) {
    currentPageNumber = pagedTransactions.pageNumber;
    renderTransactions(pagedTransactions.items); updateTransactionsSummary(pagedTransactions); renderPagination(pagedTransactions);
}

function refreshCategoryOptions() {
    renderCategories(categories, document.querySelector("#transaction-type").value, document.querySelector("#category").value);
    renderFilterCategories(categories, document.querySelector("#filter-transaction-type").value, document.querySelector("#filter-category").value);
}

function populateTransactionForm(transaction) {
    document.querySelector("#title").value = transaction.title;
    document.querySelector("#amount").value = transaction.amount;
    document.querySelector("#date").value = transaction.date.split("T")[0];
    document.querySelector("#transaction-type").value = transaction.transactionType || "Expense";
    refreshCategoryOptions();
    document.querySelector("#category").value = String(transaction.categoryId);
    document.querySelector("#payment-method").value = String(transaction.paymentMethodId);
}

if (requireAuth()) {
    document.querySelector("#logout-button").addEventListener("click", logout);
    initializePage();
}
