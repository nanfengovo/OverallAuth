<script setup lang="ts">

</script>

<template>
  <div class="app">
    <router-view />
  </div>
</template>

<style scoped>
#app {
  width: 100%;
  height: 100%;
  display: flex;
  flex-direction: column;
}

.app router-view {
  flex: 1;
  height: 90%;
  display: flex;
  flex-direction: column;
  padding: 20px;
}
</style>
