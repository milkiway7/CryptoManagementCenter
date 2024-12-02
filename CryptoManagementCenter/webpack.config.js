const path = require('path');

module.exports = {
    entry: './src/index.js', // Główne wejście Reacta
    output: {
        path: path.resolve(__dirname, 'wwwroot/js'), // Wyjściowy katalog
        filename: 'bundle.js', // Nazwa wyjściowego pliku
    },
    module: {
        rules: [
            {
                test: /\.(js|jsx)$/, // Przetwarza pliki JS/JSX
                exclude: /node_modules/,
                use: {
                    loader: 'babel-loader',
                    options: {
                        presets: ['@babel/preset-react'], // Użyj Babel do JSX
                    },
                },
            },
        ],
    },
    resolve: {
        extensions: ['.js', '.jsx'], // Obsługuje pliki JS i JSX
    },
};