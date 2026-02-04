module.exports = {
    optimization: {
        concatenateModules: false
    },
    module: {
        rules: [
            // Transpile react-resizable (uses optional chaining ?.) so Webpack/ES5 build accepts it
            {
                test: /\.js$/,
                include: /node_modules[\\/]react-resizable/,
                use: {
                    loader: 'babel-loader',
                    options: {
                        presets: [
                            ['@babel/preset-env', { targets: { ie: '11' }, bugfixes: true }]
                        ]
                    }
                }
            }
        ]
    }
};
