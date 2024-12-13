module.exports = {
    content: ["./Views/**/*.html", "./wwwroot/**/*.js"],
    theme: {
        extend: {
            colors: {
                primary: "#1F1F1F",
                secondary: "#AC99EA",
                white: "#F5F5F5"
            },
            backgroundImage: {
                'gradient-purple-magenta': 'linear-gradient(128.87deg, #9780e5 14.05%, #e666cc 89.3%)',
            },
            fontFamily: {
                spaceGrotesk: ['Space Grotesk', 'sans-serif'],
            },
        },
    },
    plugins: [],
};
