import { createCategory, deleteCategory, getCategories } from "./api.js";
import { logout, requireAuth } from "./auth.js";
import { hideLoading, showError, showLoading, showToast } from "./ui.js";

const categoryForm = document.querySelector("#category-form");
const categoriesTableBody = document.querySelector("#categories-table-body");
const emptyMessage = document.querySelector("#categories-empty-message");

if (requireAuth()) {
    document.querySelector("#logout-button").addEventListener("click", logout);
    categoryForm.addEventListener("submit", handleCategorySubmit);
    categoriesTableBody.addEventListener("click", handleCategoryTableClick);
    loadCategories();
}

async function loadCategories() {
    try {
        showLoading();
        const categories = await getCategories();
        renderCategories(categories);
    } catch (error) {
        console.error(error);
        showError("Non è stato possibile caricare le categorie.");
    } finally {
        hideLoading();
    }
}

async function handleCategorySubmit(event) {
    event.preventDefault();

    const category = {
        name: categoryForm.elements.name.value.trim(),
        transactionType: categoryForm.elements.transactionType.value
    };

    try {
        showLoading();
        await createCategory(category);
        categoryForm.reset();
        await refreshCategories();
        showToast("Categoria aggiunta correttamente.");
    } catch (error) {
        console.error(error);
        showError("Non è stato possibile aggiungere la categoria.");
    } finally {
        hideLoading();
    }
}

async function handleCategoryTableClick(event) {
    const deleteButton = event.target.closest(".delete-category-button");
    if (!deleteButton || !await confirmCategoryDeletion()) {
        return;
    }

    try {
        showLoading();
        await deleteCategory(Number(deleteButton.dataset.categoryId));
        await refreshCategories();
        showToast("Categoria eliminata.");
    } catch (error) {
        console.error(error);
        showError("Non è stato possibile eliminare la categoria.");
    } finally {
        hideLoading();
    }
}

async function refreshCategories() {
    const categories = await getCategories();
    renderCategories(categories);
}

function renderCategories(categories) {
    categoriesTableBody.innerHTML = "";
    emptyMessage.hidden = categories.length !== 0;

    for (const category of categories) {
        const row = document.createElement("tr");
        const isIncome = category.transactionType === "Income";

        row.innerHTML = `
            <td><span class="category-pill"></span></td>
            <td><span class="transaction-type ${isIncome ? "transaction-type-income" : "transaction-type-expense"}"></span></td>
            <td class="text-end"><button type="button" class="btn btn-outline-danger btn-sm delete-category-button" data-category-id="${category.id}"><i class="bi bi-trash3" aria-hidden="true"></i><span class="visually-hidden">Elimina categoria</span></button></td>`;

        row.querySelector(".category-pill").textContent = category.name;
        row.querySelector(".transaction-type").textContent = isIncome ? "Entrata" : "Uscita";
        categoriesTableBody.appendChild(row);
    }
}

function confirmCategoryDeletion() {
    const modalElement = document.querySelector("#delete-category-modal");
    const confirmButton = document.querySelector("#confirm-delete-category-button");

    return new Promise((resolve) => {
        const modal = window.bootstrap.Modal.getOrCreateInstance(modalElement);
        let confirmed = false;
        const handleConfirmation = () => {
            confirmed = true;
            modal.hide();
        };
        const handleModalHidden = () => {
            confirmButton.removeEventListener("click", handleConfirmation);
            resolve(confirmed);
        };

        confirmButton.addEventListener("click", handleConfirmation);
        modalElement.addEventListener("hidden.bs.modal", handleModalHidden, { once: true });
        modal.show();
    });
}
