import webpack from 'webpack';
import HtmlWebpackPlugin from 'html-webpack-plugin';
import path from 'path';

const ROOT_PATH = path.resolve('./');
const SERVER_URL = 'DEV';


export default {
    entry: [
        'webpack/hot/dev-server',
        'webpack-dev-server/client?http://0.0.0.0:8080',
        path.resolve(ROOT_PATH, 'src/index')
    ],
    resolve: {
        extensions: ['', '.js']
    },
    output: {
        path: path.resolve(ROOT_PATH, 'build'),
        filename: 'app.bundle.js'
    },
    devtool: 'cheap-eval-source-map',
    devServer: {
        historyApiFallback: true,
        https: false
    },
    module: {
        loaders: [
            {
                test: /\.js$/,
                loader: 'babel',
                exclude: /node_modules/
            },
            {
                test: /\.scss$/,
                loader: 'style!css!sass',
                exclude: /node_modules/
            },
            {
                test: /\.js$/,
                loader: 'webpack-replace',
                query: {
                    search: '##server_url##',
                    replace: SERVER_URL
                }
            }
        ]
    },
    plugins: [
        new webpack.HotModuleReplacementPlugin(),
        new webpack.NoErrorsPlugin(),
        new HtmlWebpackPlugin({
            template: 'index.html',
            inject:   true,
            hash: true,
            filename: 'index.html'
        })
    ]
};