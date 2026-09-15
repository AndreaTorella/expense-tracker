import { loadDashboard, renderDashboard, formatMonth } from "./dashboard.js";
import { logout, requireAuth } from "./auth.js";
import { hideLoading, showError, showLoading } from "./ui.js";

const today = new Date();
const currentMonth = new Date(today.getFullYear(), today.getMonth(), 1);
let selectedMonth = new Date(currentMonth);

async function initializeDashboard() {
    document.querySelector("#previous-month-button").addEventListener("click", () => changeMonth(-1));
    document.querySelector("#next-month-button").addEventListener("click", () => changeMonth(1));
    await refreshDashboard();
}

async function changeMonth(offset) {
    const nextSelection = new Date(selectedMonth.getFullYear(), selectedMonth.getMonth() + offset, 1);
    if (nextSelection > currentMonth) return;
    selectedMonth = nextSelection;
    await refreshDashboard();
}

async function refreshDashboard() {
    updateMonthNavigation();
    try {
        showLoading();
        const { summary, latestTransactions } = await loadDashboard(selectedMonth.getFullYear(), selectedMonth.getMonth() + 1);
        renderDashboard(summary, latestTransactions, selectedMonth);
    } catch (error) {
        console.error(error);
        showError("Non è stato possibile caricare la dashboard.");
    } finally {
        hideLoading();
    }
}

function updateMonthNavigation() {
    document.querySelector("#selected-month").textContent = formatMonth(selectedMonth);
    document.querySelector("#next-month-button").disabled = selectedMonth >= currentMonth;
}

if (requireAuth()) {
    document.querySelector("#logout-button").addEventListener("click", logout);
    initializeDashboard();
}
