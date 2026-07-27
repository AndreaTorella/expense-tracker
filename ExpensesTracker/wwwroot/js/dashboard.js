import { getDashboardSummary, getTransactions } from "./api.js";

const chartColors = ["#176b5f", "#c78b36", "#537895", "#9a5b5b", "#7d6c9d", "#559176"];
let categoryChart;
let monthlyChart;

export async function loadDashboard(year, month) {
    const [summary, latestTransactions] = await Promise.all([
        getDashboardSummary(year, month),
        getTransactions({ PageNumber: 1, PageSize: 5, TransactionType: "Expense", TransactionSortBy: "Date", SortDirection: "Desc" })
    ]);

    return { summary, latestTransactions: latestTransactions.items };
}

export function renderDashboard(summary, latestTransactions, selectedDate) {
    const previousDate = new Date(selectedDate.getFullYear(), selectedDate.getMonth() - 1, 1);
    document.querySelector("#current-month-expenses").textContent = formatCurrency(summary.currentMonthTotalExpenses);
    document.querySelector("#current-month-incomes").textContent = formatCurrency(summary.currentMonthTotalIncomes);
    renderBalance(summary.currentMonthBalance);
    document.querySelector("#previous-month-expenses").textContent = formatCurrency(summary.previousMonthTotalExpenses);
    document.querySelector("#current-month-label").textContent = formatMonth(selectedDate);
    document.querySelector("#current-month-income-label").textContent = formatMonth(selectedDate);
    document.querySelector("#current-month-balance-label").textContent = formatMonth(selectedDate);
    document.querySelector("#previous-month-label").textContent = formatMonth(previousDate);
    renderDifference(summary.differenceExpensePercentage);
    renderCategoryChart(summary.categoryTotals);
    renderMonthlyChart(summary.monthlyTotals);
    renderLatestExpenses(latestTransactions);
}

export function formatMonth(date) {
    return new Intl.DateTimeFormat("it-IT", { month: "long", year: "numeric" }).format(date);
}

function renderDifference(differencePercentage) {
    const element = document.querySelector("#expense-difference-percentage");
    const description = document.querySelector("#expense-difference-description");

    if (differencePercentage === null || differencePercentage === undefined) {
        element.textContent = "N/D";
        element.className = "is-neutral";
        description.textContent = "Non confrontabile con il mese precedente";
        return;
    }

    const sign = differencePercentage > 0 ? "+" : "";
    element.textContent = `${sign}${Number(differencePercentage).toLocaleString("it-IT", { maximumFractionDigits: 2 })}%`;
    element.className = differencePercentage > 0 ? "is-increase" : "is-decrease";
    description.textContent = "Rispetto al mese precedente";
}

function renderBalance(balance) {
    const element = document.querySelector("#current-month-balance");
    const numericBalance = Number(balance);

    element.textContent = formatCurrency(numericBalance);
    element.className = numericBalance > 0
        ? "is-positive"
        : numericBalance < 0
            ? "is-negative"
            : "is-neutral";
}

function renderCategoryChart(categoryTotals) {
    const canvas = document.querySelector("#category-chart");
    const emptyMessage = document.querySelector("#category-chart-empty");
    categoryChart?.destroy();

    if (!categoryTotals.length) {
        canvas.hidden = true;
        emptyMessage.hidden = false;
        return;
    }

    canvas.hidden = false;
    emptyMessage.hidden = true;
    categoryChart = new Chart(canvas, {
        type: "doughnut",
        data: {
            labels: categoryTotals.map((item) => item.categoryName),
            datasets: [{ data: categoryTotals.map((item) => item.total), backgroundColor: chartColors, borderWidth: 0 }]
        },
        options: { cutout: "68%", plugins: { legend: { position: "bottom", labels: { usePointStyle: true, padding: 16 } }, tooltip: { callbacks: { label: (context) => `${context.label}: ${formatCurrency(context.raw)}` } } } }
    });
}

function renderMonthlyChart(monthlyTotals) {
    const canvas = document.querySelector("#monthly-chart");
    monthlyChart?.destroy();
    monthlyChart = new Chart(canvas, {
        type: "bar",
        data: {
            labels: monthlyTotals.map((item) => formatMonth(new Date(item.year, item.month - 1, 1))),
            datasets: [{ label: "Spese", data: monthlyTotals.map((item) => item.total), backgroundColor: "#176b5f", borderRadius: 6, maxBarThickness: 44 }]
        },
        options: { maintainAspectRatio: false, scales: { y: { beginAtZero: true, ticks: { callback: (value) => formatCurrency(value) }, grid: { color: "#edf1f5" } }, x: { grid: { display: false } } }, plugins: { legend: { display: false }, tooltip: { callbacks: { label: (context) => formatCurrency(context.raw) } } } }
    });
}

function renderLatestExpenses(transactions) {
    const container = document.querySelector("#latest-expenses-list");
    container.innerHTML = "";
    if (!transactions.length) {
        container.innerHTML = '<p class="latest-expenses-empty">Non sono ancora presenti spese.</p>';
        return;
    }

    transactions.forEach((transaction) => {
        const item = document.createElement("div");
        item.className = "latest-expense-item";
        item.innerHTML = `<div><strong></strong><span></span></div><b></b>`;
        item.querySelector("strong").textContent = transaction.title;
        item.querySelector("span").textContent = `${transaction.categoryName} · ${new Date(transaction.date).toLocaleDateString("it-IT")}`;
        item.querySelector("b").textContent = formatCurrency(transaction.amount);
        container.appendChild(item);
    });
}

function formatCurrency(value) {
    return Number(value).toLocaleString("it-IT", { style: "currency", currency: "EUR" });
}
