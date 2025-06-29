<template>
    <div class="menu">
        <div class="chart-container">
            <div ref="menuChart" style="width: 100%; height: 400px;"></div>
        </div>
        <div class="logo">
            <img src="../../assets/img/logo.jpg" alt="">
        </div>
        <div class="menu-container">
            <el-menu :default-active="activeMenu" class="el-menu-vertical" background-color="#545c64" text-color="#fff"
                active-text-color="#ffd04b" router>
                <template v-for="item in filteredMenu" :key="item.id">
                    <menu-item :item="item" />
                </template>
            </el-menu>
        </div>
    </div>

</template>

<script setup lang="ts">
import * as echarts from 'echarts';
import { ref, computed, onMounted, watch } from 'vue'
import axios from 'axios'
import { useRoute } from 'vue-router'
import MenuItem from './MenuItem.vue'

// 定义菜单项类型
interface MenuItem {
    id: number
    name: string
    icon: string
    url: string
    roles: string[]
    children?: MenuItem[]
    isEnable: boolean
}

const route = useRoute()
const menuItems = ref<MenuItem[]>([])

// 获取当前激活菜单项（根据路由路径）
const activeMenu = computed(() => route.path)

// 过滤可用且有权限的菜单
const filteredMenu = computed(() => {
    const userRoles = ['admin'] // 从Vuex/Pinia获取实际用户角色
    return menuItems.value.filter(item =>
        item.isEnable &&
        (item.roles.length === 0 || item.roles.some(r => userRoles.includes(r)))
    )
})

// 从后端获取菜单
const fetchMenu = async () => {
    try {
        const userName = localStorage.getItem('username') || ''; // 动态变量
        if (!userName) {
            console.error('未找到用户名，请先登录')
            return
        }
        console.log('正在加载菜单数据...当前登录的用户为:', userName)
        const res = await axios.get("http://localhost:5141/api/Menu/GetMenuByRole", {
            params: {
                userName: userName
            }
        });
        console.log('菜单数据:', res.data.data)
        menuItems.value = res.data.data
        localStorage.setItem('menu', JSON.stringify(menuItems.value)) // 存储菜单到本地
    } catch (err) {
        console.error('菜单加载失败', err)
    }
}

const menuChart = ref<HTMLElement>();

onMounted(() => {
    fetchMenu();
})

watch(menuItems, () => {
    initMenuChart();
}, { deep: true });

function convertToTreeData(menus: MenuItem[]): any {
    return {
        name: '菜单结构',
        children: menus.map(menu => ({
            name: menu.name,
            children: menu.children?.map(child => ({
                name: child.name,
                itemStyle: {
                    color: child.isEnable ? '#67C23A' : '#F56C6C'
                }
            })) || [],
            itemStyle: {
                color: menu.isEnable ? '#67C23A' : '#F56C6C'
            }
        }))
    };
}

function initMenuChart() {
    if (!menuChart.value || menuItems.value.length === 0) return;

    const chart = echarts.init(menuChart.value);
    const treeData = convertToTreeData(menuItems.value);

    chart.setOption({
        title: {
            text: '菜单层级结构',
            left: 'center'
        },
        tooltip: {
            trigger: 'item',
            triggerOn: 'mousemove'
        },
        series: [
            {
                type: 'tree',
                data: [treeData],
                symbol: 'roundRect',
                symbolSize: 7,
                orient: 'vertical',
                expandAndCollapse: true,
                initialTreeDepth: 3,
                label: {
                    position: 'top',
                    rotate: 0,
                    verticalAlign: 'middle',
                    align: 'right',
                    fontSize: 12
                },
                leaves: {
                    label: {
                        position: 'right',
                        verticalAlign: 'middle',
                        align: 'left'
                    }
                },
                emphasis: {
                    focus: 'descendant'
                },
                animationDurationUpdate: 750
            }
        ]
    });
}


</script>

<style lang="less" scoped>
.chart-container {
    background: #fff;
    padding: 20px;
    margin-bottom: 20px;
    border-radius: 20px;
}

.menu {
    height: 100vh;
    overflow-y: hidden;
    background-color: #545c64;
    overflow-x: hidden;
}

.logo {
    height: 60px;
    width: 200px;
}

.logo img {
    width: 100%;
    height: 100%;
    object-fit: contain;
    /* 保持比例，可能留白 */
}

.menu-container {
    height: 100vh;
    overflow-y: hidden;
    background-color: #545c64;
    overflow-x: hidden;

    .el-menu-vertical {
        border-right: none;
        height: 100%;

        &:not(.el-menu--collapse) {
            width: 220px;
        }
    }
}
</style>