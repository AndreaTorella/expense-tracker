import { getDashboardSummary, getExpenses } from "./api.js";

const chartColors = ["#176b5f", "#c78b36", "#537895", "#9a5b5b", "#7d6c9d", "#559176"];
let categoryChart;
let monthlyChart;

export async function loadDashboard(year, month) {
    const [summary, latestExpenses] = await Promise.all([
        getDashboardSummary(year, month),
        getExpenses({ PageNumber: 1, PageSize: 5, ExpenseSortBy: "Date", SortDirection: "Desc" })
    ]);

    return { summary, latestExpenses: latestExpenses.items };
}

export function renderDashboard(summary, latestExpenses, selectedDate) {
    const previousDate = new Date(selectedDate.getFullYear(), selectedDate.getMonth() - 1, 1);
    document.querySelector("#current-month-total").textContent = formatCurrency(summary.currentMonthTotal);
    document.querySelector("#previous-month-total").textContent = formatCurrency(summary.previousMonthTotal);
    document.querySelector("#current-month-label").textContent = formatMonth(selectedDate);
    document.querySelector("#previous-month-label").textContent = formatMonth(previousDate);
    renderDifference(summary.differencePercentage);
    renderCategoryChart(summary.categoryTotals);
    renderMonthlyChart(summary.monthlyTotals);
    renderLatestExpenses(latestExpenses);
}

export function formatMonth(date) {
    return new Intl.DateTimeFormat("it-IT", { month: "long", year: "numeric" }).format(date);
}

function renderDifference(differencePercentage) {
    const element = document.querySelector("#difference-percentage");
    const description = document.querySelector("#difference-description");

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

function renderLatestExpenses(expenses) {
    const container = document.querySelector("#latest-expenses-list");
    container.innerHTML = "";
    if (!expenses.length) {
        container.innerHTML = '<p class="latest-expenses-empty">Non sono ancora presenti spese.</p>';
        return;
    }

    expenses.forEach((expense) => {
        const item = document.createElement("div");
        item.className = "latest-expense-item";
        item.innerHTML = `<div><strong></strong><span></span></div><b></b>`;
        item.querySelector("strong").textContent = expense.title;
        item.querySelector("span").textContent = `${expense.categoryName} · ${new Date(expense.date).toLocaleDateString("it-IT")}`;
        item.querySelector("b").textContent = formatCurrency(expense.amount);
        container.appendChild(item);
    });
}

function formatCurrency(value) {
    return Number(value).toLocaleString("it-IT", { style: "currency", currency: "EUR" });
}
