<template>
    <div class="home">
        <h2>home</h2>
    </div>
</template>
<script setup lang="ts">
option = {
    series: [
        {
            type: 'pie',
            data: [
                {
                    value: 335,
                    name: '直接访问'
                },
                {
                    value: 234,
                    name: '联盟广告'
                },
                {
                    value: 1548,
                    name: '搜索引擎'
                }
            ]
        }
    ]
};
</script>

<style lang="less" scoped>
.home {}
</style>