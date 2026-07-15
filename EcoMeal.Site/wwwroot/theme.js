window.ecoMealTheme = {
    get: function () {
        return localStorage.getItem('ecoMeal.theme');
    },
    set: function (theme) {
        localStorage.setItem('ecoMeal.theme', theme);
    }
};
