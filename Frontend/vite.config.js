import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import vueDevTools from 'vite-plugin-vue-devtools'

// https://vite.dev/config/
export default defineConfig({
  plugins: [
    vue(),
    vueDevTools(),
  ],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    },
  },
  build: {
    // Optimizaciones de bundle
    target: 'esnext',
    minify: 'terser',
    cssMinify: true,
    
    // Code splitting optimizado
    rollupOptions: {
      output: {
        manualChunks: {
          // Separar vendor libraries
          'vue-vendor': ['vue', 'vue-router'],
          'ui-vendor': ['bootstrap'],
          'http-vendor': ['axios']
        },
        // Nombres de archivos optimizados
        chunkFileNames: 'js/[name]-[hash].js',
        entryFileNames: 'js/[name]-[hash].js',
        assetFileNames: (assetInfo) => {
          const info = assetInfo.name.split('.')
          const ext = info[info.length - 1]
          if (/\.(css)$/.test(assetInfo.name)) {
            return `css/[name]-[hash].${ext}`
          }
          if (/\.(png|jpe?g|svg|gif|tiff|bmp|ico)$/i.test(assetInfo.name)) {
            return `img/[name]-[hash].${ext}`
          }
          return `assets/[name]-[hash].${ext}`
        }
      }
    },
    
    // Configuración de terser para mejor minificación
    terserOptions: {
      compress: {
        drop_console: true, // Remover console.log en producción
        drop_debugger: true,
        pure_funcs: ['console.log', 'console.info', 'console.debug']
      }
    },
    
    // Configuración de chunks
    chunkSizeWarningLimit: 1000,
    
    // Source maps solo en desarrollo
    sourcemap: false
  },
  
  // Optimizaciones de desarrollo
  server: {
    hmr: {
      overlay: false
    }
  },
  
  // Optimizaciones de CSS
  css: {
    devSourcemap: false
  }
})
