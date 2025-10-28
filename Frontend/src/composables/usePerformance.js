import { ref, onMounted, onUnmounted } from 'vue'

export function usePerformance() {
  const metrics = ref({
    loadTime: 0,
    renderTime: 0,
    apiCalls: [],
    memoryUsage: 0,
    connectionType: 'unknown'
  })

  const startTime = ref(0)
  const observer = ref(null)

  // Medir tiempo de carga inicial
  const measureLoadTime = () => {
    if (performance.timing) {
      const loadTime = performance.timing.loadEventEnd - performance.timing.navigationStart
      metrics.value.loadTime = loadTime
    }
  }

  // Medir tiempo de renderizado de componente
  const startRender = () => {
    startTime.value = performance.now()
  }

  const endRender = () => {
    if (startTime.value > 0) {
      metrics.value.renderTime = performance.now() - startTime.value
      startTime.value = 0
    }
  }

  // Monitorear llamadas API
  const trackApiCall = (url, method, duration, status) => {
    metrics.value.apiCalls.push({
      url,
      method,
      duration,
      status,
      timestamp: new Date().toISOString()
    })

    // Mantener solo las últimas 50 llamadas
    if (metrics.value.apiCalls.length > 50) {
      metrics.value.apiCalls = metrics.value.apiCalls.slice(-50)
    }
  }

  // Obtener información de memoria (si está disponible)
  const updateMemoryUsage = () => {
    if (performance.memory) {
      metrics.value.memoryUsage = {
        used: Math.round(performance.memory.usedJSHeapSize / 1024 / 1024),
        total: Math.round(performance.memory.totalJSHeapSize / 1024 / 1024),
        limit: Math.round(performance.memory.jsHeapSizeLimit / 1024 / 1024)
      }
    }
  }

  // Obtener tipo de conexión
  const updateConnectionInfo = () => {
    if (navigator.connection) {
      metrics.value.connectionType = {
        effectiveType: navigator.connection.effectiveType,
        downlink: navigator.connection.downlink,
        rtt: navigator.connection.rtt
      }
    }
  }

  // Observar cambios en el DOM para detectar re-renders
  const observeDOM = () => {
    if (typeof MutationObserver !== 'undefined') {
      observer.value = new MutationObserver((mutations) => {
        const significantChanges = mutations.filter(mutation => 
          mutation.type === 'childList' && mutation.addedNodes.length > 0
        )
        
        if (significantChanges.length > 0) {
          // Registrar re-render significativo
          console.log('DOM re-render detectado:', significantChanges.length, 'cambios')
        }
      })

      observer.value.observe(document.body, {
        childList: true,
        subtree: true,
        attributes: false
      })
    }
  }

  // Obtener métricas de Web Vitals
  const getWebVitals = () => {
    // Core Web Vitals
    const vitals = {}

    // LCP (Largest Contentful Paint)
    if ('PerformanceObserver' in window) {
      try {
        const lcpObserver = new PerformanceObserver((list) => {
          const entries = list.getEntries()
          const lastEntry = entries[entries.length - 1]
          vitals.lcp = Math.round(lastEntry.startTime)
        })
        lcpObserver.observe({ entryTypes: ['largest-contentful-paint'] })
      } catch (e) {
        console.warn('LCP measurement not supported')
      }

      // FID (First Input Delay)
      try {
        const fidObserver = new PerformanceObserver((list) => {
          const entries = list.getEntries()
          entries.forEach((entry) => {
            vitals.fid = Math.round(entry.processingStart - entry.startTime)
          })
        })
        fidObserver.observe({ entryTypes: ['first-input'] })
      } catch (e) {
        console.warn('FID measurement not supported')
      }
    }

    return vitals
  }

  // Generar reporte de performance
  const generateReport = () => {
    const report = {
      timestamp: new Date().toISOString(),
      metrics: { ...metrics.value },
      webVitals: getWebVitals(),
      userAgent: navigator.userAgent,
      viewport: {
        width: window.innerWidth,
        height: window.innerHeight
      }
    }

    return report
  }

  // Log de performance para debugging
  const logPerformance = () => {
    const report = generateReport()
    console.group('📊 Performance Report')
    console.log('Load Time:', report.metrics.loadTime, 'ms')
    console.log('Render Time:', report.metrics.renderTime, 'ms')
    console.log('Memory Usage:', report.metrics.memoryUsage)
    console.log('Connection:', report.metrics.connectionType)
    console.log('API Calls:', report.metrics.apiCalls.length)
    console.groupEnd()
  }

  onMounted(() => {
    measureLoadTime()
    updateMemoryUsage()
    updateConnectionInfo()
    observeDOM()

    // Actualizar métricas cada 30 segundos
    const interval = setInterval(() => {
      updateMemoryUsage()
      updateConnectionInfo()
    }, 30000)

    onUnmounted(() => {
      clearInterval(interval)
      if (observer.value) {
        observer.value.disconnect()
      }
    })
  })

  return {
    metrics,
    startRender,
    endRender,
    trackApiCall,
    generateReport,
    logPerformance
  }
}