import rollup      from 'rollup'
import nodeResolve from 'rollup-plugin-node-resolve'
import commonjs    from 'rollup-plugin-commonjs';
import uglify      from 'rollup-plugin-uglify'

export default {
entry: 'Login/BootstraperAOT.js',
  dest: 'dist/build.js', // output a single application bundle
  sourceMap: false,
  format: 'iife',
  plugins: [

commonjs({
namedExports: {
          include: ['node_modules/rxjs/**','node_modules/ng2-charts/**'],

'node_modules/ng2-charts/ng2-charts.js': ['ChartsModule']
}
}),
nodeResolve({
module: true,
jsnext: true,
main: true,
browser: true,
extensions: ['.js']
}),


    
     
      uglify(),
  ]
}
