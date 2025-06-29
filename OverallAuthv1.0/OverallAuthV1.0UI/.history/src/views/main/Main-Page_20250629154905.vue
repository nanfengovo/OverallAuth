<template>
    <div ref="chartContainer" :style="{ width: '100%', height: '400px' }" />
</template>

<script lang="ts" setup>
import * as echarts from 'echarts/core';
import { BarChart, LineChart } from 'echarts/charts';
import {
    TitleComponent,
    TooltipComponent,
    GridComponent,
    LegendComponent,
    CanvasRenderer
} from 'echarts/components';
import { onMounted, onUnmounted, ref, watch } from 'vue';
import type { ECharts, EChartsOption } from 'echarts';

// 按需引入模块（减小打包体积）
echarts.use([
    TitleComponent,
    TooltipComponent,
    GridComponent,
    LegendComponent,
    BarChart,
    LineChart,
    CanvasRenderer
]);

const chartContainer = ref<HTMLDivElement | null>(null);
let chartInstance: ECharts | null = null;

// 图表配置项（TypeScript 类型声明）
const chartOption = ref<EChartsOption>({
    title: {
        text: '销售数据统计',
        left: 'center'
    },
    tooltip: {
        trigger: 'axis'
    },
    legend: {
        data: ['线上销量', '门店销量'],
        bottom: 0
    },
    xAxis: {
        type: 'category',
        data: ['1月', '2月', '3月', '4月', '5月', '6月']
    },
    yAxis: { type: 'value' },
    series: [
        { name: '线上销量', type: 'bar', data: [120, 132, 101, 134, 90, 230] },
        { name: '门店销量', type: 'line', data: [220, 182, 191, 234, 290, 330] }
    ]
});

// 初始化图表
const initChart = () => {
    if (!chartContainer.value) return;

    // 销毁旧实例避免重复渲染
    chartInstance?.dispose();

    // 初始化新实例
    chartInstance = echarts.init(chartContainer.value);
    chartInstance.setOption(chartOption.value);

    // 添加窗口自适应监听
    window.addEventListener('resize', handleResize);
};

// 响应式更新图表
watch(chartOption, (newOption) => {
    if (chartInstance) {
        chartInstance.setOption(newOption);
    }
});

// 处理容器大小变化
const handleResize = () => {
    chartInstance?.resize();
};

onMounted(() => {
    initChart();
});

onUnmounted(() => {
    // 清理资源
    chartInstance?.dispose();
    window.removeEventListener('resize', handleResize);
});
</script>