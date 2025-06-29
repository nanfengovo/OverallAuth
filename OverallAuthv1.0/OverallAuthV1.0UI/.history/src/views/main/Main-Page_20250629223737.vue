<script setup lang="ts">
import { ref, onMounted } from 'vue';
import * as echarts from 'echarts';
import axios from 'axios';

// Chart refs
const userChartRef = ref<HTMLDivElement>();
const roleChartRef = ref<HTMLDivElement>();
const menuChartRef = ref<HTMLDivElement>();
const relationChartRef = ref<HTMLDivElement>();

// Data
const userData = ref<any[]>([]);
const roleData = ref<any[]>([]);
const menuData = ref<any[]>([]);

// Fetch all data
const fetchData = async () => {
    try {
        const [rolesRes, menusRes] = await Promise.all([
            axios.get("http://127.0.0.1:5141/api/OverallAuth/GetAllRole"),
            axios.get("http://localhost:5141/api/Menu/GetMenuByRole", {
                params: { userName: localStorage.getItem('username') || '' }
            })
        ]);

        roleData.value = rolesRes.data.data;
        menuData.value = menusRes.data.data;
        initCharts();
    } catch (error) {
        console.error('数据加载失败:', error);
    }
};

// Initialize all charts
const initCharts = () => {
    initUserChart();
    initRoleChart();
    initMenuChart();
    initRelationChart();
};

// User statistics chart
const initUserChart = () => {
    if (!userChartRef.value) return;
    const chart = echarts.init(userChartRef.value);

    chart.setOption({
        title: { text: '用户统计', left: 'center' },
        tooltip: { trigger: 'item' },
        legend: { orient: 'vertical', left: 'left' },
        series: [{
            name: '用户分布',
            type: 'pie',
            radius: ['40%', '70%'],
            avoidLabelOverlap: false,
            itemStyle: {
                borderRadius: 10,
                borderColor: '#fff',
                borderWidth: 2
            },
            label: {
                show: false,
                position: 'center'
            },
            emphasis: {
                label: { show: true, fontSize: 18, fontWeight: 'bold' }
            },
            labelLine: { show: false },
            data: [
                { value: 1048, name: '管理员' },
                { value: 735, name: '普通用户' },
                { value: 580, name: '访客' }
            ],
            animationType: 'scale',
            animationEasing: 'elasticOut',
            animationDelay: function (idx: number) {
                return Math.random() * 200;
            }
        }]
    });
};

// Role distribution chart
const initRoleChart = () => {
    if (!roleChartRef.value || !roleData.value.length) return;
    const chart = echarts.init(roleChartRef.value);
    const enabledCount = roleData.value.filter((r: any) => r.isEnable).length;
    const disabledCount = roleData.value.filter((r: any) => !r.isEnable).length;

    chart.setOption({
        title: { text: '角色状态分布', left: 'center' },
        tooltip: { trigger: 'item' },
        legend: { top: '5%', left: 'center' },
        series: [{
            name: '角色状态',
            type: 'pie',
            radius: ['40%', '70%'],
            center: ['50%', '60%'],
            data: [
                { value: enabledCount, name: '已启用', itemStyle: { color: '#67C23A' } },
                { value: disabledCount, name: '已禁用', itemStyle: { color: '#F56C6C' } }
            ],
            emphasis: {
                itemStyle: {
                    shadowBlur: 10,
                    shadowOffsetX: 0,
                    shadowColor: 'rgba(0, 0, 0, 0.5)'
                }
            },
            animationType: 'scale',
            animationEasing: 'elasticOut',
            animationDelay: function (idx: number) {
                return Math.random() * 200;
            }
        }]
    });
};

// Menu hierarchy chart
const initMenuChart = () => {
    if (!menuChartRef.value || !menuData.value.length) return;
    const chart = echarts.init(menuChartRef.value);

    const convertToTreeData = (menus: any[]) => {
        return {
            name: '菜单结构',
            children: menus.map(menu => ({
                name: menu.name,
                children: menu.children?.map((child: any) => ({
                    name: child.name,
                    itemStyle: { color: child.isEnable ? '#67C23A' : '#F56C6C' }
                })) || [],
                itemStyle: { color: menu.isEnable ? '#67C23A' : '#F56C6C' }
            }))
        };
    };

    chart.setOption({
        title: { text: '菜单层级', left: 'center' },
        tooltip: { trigger: 'item', triggerOn: 'mousemove' },
        series: [{
            type: 'tree',
            data: [convertToTreeData(menuData.value)],
            symbol: 'roundRect',
            symbolSize: 10,
            orient: 'vertical',
            expandAndCollapse: true,
            initialTreeDepth: 2,
            label: {
                position: 'top',
                rotate: 0,
                verticalAlign: 'middle',
                align: 'right',
                fontSize: 12
            },
            leaves: { label: { position: 'right', verticalAlign: 'middle', align: 'left' } },
            emphasis: { focus: 'descendant' },
            animationDurationUpdate: 750,
            itemStyle: {
                borderColor: '#409EFF',
                borderWidth: 1
            }
        }]
    });
};

// Role-menu relationship graph
const initRelationChart = () => {
    if (!relationChartRef.value || !roleData.value.length || !menuData.value.length) return;
    const chart = echarts.init(relationChartRef.value);

    const nodes = [
        ...roleData.value.map((role: any) => ({
            id: role.name,
            name: role.name,
            symbolSize: 30,
            category: 0,
            itemStyle: { color: role.isEnable ? '#67C23A' : '#F56C6C' }
        })),
        ...menuData.value.map((menu: any) => ({
            id: menu.name,
            name: menu.name,
            symbolSize: 20,
            category: 1,
            itemStyle: { color: menu.isEnable ? '#409EFF' : '#F56C6C' }
        }))
    ];

    const links = roleData.value.flatMap((role: any) =>
        role.menusName?.map((menuName: string) => ({
            source: role.name,
            target: menuName
        })) || []
    );

    chart.setOption({
        title: { text: '角色-菜单关系', left: 'center' },
        tooltip: {},
        legend: {
            data: ['角色', '菜单'],
            top: 'bottom'
        },
        animationDuration: 1500,
        animationEasingUpdate: 'quinticInOut',
        series: [{
            name: '关系图',
            type: 'graph',
            layout: 'force',
            data: nodes,
            links: links,
            categories: [
                { name: '角色' },
                { name: '菜单' }
            ],
            roam: true,
            label: {
                show: true,
                position: 'right'
            },
            force: {
                repulsion: 100,
                edgeLength: [50, 100]
            },
            emphasis: {
                focus: 'adjacency',
                lineStyle: {
                    width: 3
                }
            },
            lineStyle: {
                opacity: 0.9,
                width: 1.5,
                curveness: 0.2
            }
        }]
    });
};

onMounted(() => {
    fetchData();
    window.addEventListener('resize', () => {
        [userChartRef, roleChartRef, menuChartRef, relationChartRef].forEach(ref => {
            ref.value && echarts.getInstanceByDom(ref.value)?.resize();
        });
    });
});
</script>

<template>
    <div class="dashboard-container">
        <div class="chart-card">
            <div ref="userChartRef" class="chart"></div>
        </div>
        <div class="chart-card">
            <div ref="roleChartRef" class="chart"></div>
        </div>
        <div class="chart-card">
            <div ref="menuChartRef" class="chart"></div>
        </div>
        <div class="chart-card">
            <div ref="relationChartRef" class="chart"></div>
        </div>
    </div>
</template>

<style scoped>
.dashboard-container {
    display: grid;
    grid-template-columns: repeat(2, 1fr);
    grid-template-rows: repeat(2, 1fr);
    gap: 20px;
    height: calc(100vh - 40px);
    padding: 20px;
    background-color: #f5f7fa;
}

.chart-card {
    background: white;
    border-radius: 12px;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
    padding: 16px;
    transition: all 0.3s ease;
}

.chart-card:hover {
    transform: translateY(-5px);
    box-shadow: 0 6px 16px rgba(0, 0, 0, 0.15);
}

.chart {
    width: 100%;
    height: 100%;
    min-height: 300px;
}
</style>
</style>