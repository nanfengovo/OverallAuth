<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue';
import * as echarts from 'echarts/core'; // 核心模块
// 1. 从正确路径导入渲染器
import { CanvasRenderer } from 'echarts/renderers'; // [!code focus]
// 2. 按需引入图表类型
import { BarChart, LineChart } from 'echarts/charts';
// 3. 引入组件
import {
    GridComponent,
    TooltipComponent,
    LegendComponent
} from 'echarts/components';

// 注册模块
echarts.use([
    CanvasRenderer, // [!code focus]
    BarChart,
    LineChart,
    GridComponent,
    TooltipComponent,
    LegendComponent
]);

const chartRef = ref<HTMLDivElement>();
let chartInstance: echarts.ECharts | null = null;

onMounted(() => {
    if (!chartRef.value) return;

    // 初始化图表（明确指定 Canvas 渲染器）
    chartInstance = echarts.init(chartRef.value, null, {
        renderer: 'canvas' // 可选，默认就是canvas
    });

    chartInstance.setOption({
        xAxis: { data: ['A', 'B', 'C'] },
        yAxis: {},
        series: [{ type: 'bar', data: [10, 20, 30] }]
    });
});

onUnmounted(() => {
    chartInstance?.dispose(); // 销毁实例
});
</script>

<template>
    <div class="chart" ref="chartRef" />
    <div class="chart" ref="chartRef" />
    <div class="chart" ref="chartRef" />
    <div class="chart" ref="chartRef" />
</template>
<style scoped>
.chart {
    width: 25%;
    height: 25%;
    border: 1px solid #ccc
}
</style>