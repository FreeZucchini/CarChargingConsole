<script>
    let { charge = 0 } = $props()
</script>

<style>
  .battery-bar {
    width: 200px;
    height: 40px;
    border: 3px solid black;
    border-radius: 3px;
    overflow: hidden;
    margin: 0 auto;
  }

  .fill {
    height: 100%;
    background: black;
    transition: width 0.3s ease;
  }
</style>

<div class="battery-bar">
    <div class="fill" style={`width: ${charge * 2}%`}></div>
</div>

