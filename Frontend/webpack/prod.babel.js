import webpack from 'webpack';
import path from 'path';
import HtmlWebpackPlugin from 'html-webpack-plugin';

const ROOT_PATH = path.resolve('./');
const SERVER_URL = 'PRODUCTION';

export default {
    entry: {
        index: [
            path.resolve(ROOT_PATH, 'src/index')
        ]
    },
    resolve: {
        extensions: ['', '.js']
    },
    output: {
        path: path.resolve(ROOT_PATH, 'build'),
        filename: "[name].min.js"
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
    devtool: 'hidden-source-map',
    plugins: [
        new webpack.DefinePlugin({
            "process.env": {
                NODE_ENV: JSON.stringify("production")
            },
            __DEV__: false
        }),
        new webpack.optimize.UglifyJsPlugin(),
        new HtmlWebpackPlugin({
            template: 'index.html',
            filename: 'index.html',
            hash: true,
            inject: true
        })
    ]
};