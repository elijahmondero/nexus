const fs = require('fs');
const path = require('path');

const command = process.argv[2];
const name = process.argv[3];

if (!command || !name) {
  console.log('Usage: node cli/scaffold.js <command> <Name>');
  process.exit(1);
}

const timestamp = Math.floor(Date.now() / 1000);

const templates = {
  crud: {
    api: [
      { tpl: 'api/crud-controller.tpl', out: `backend/src/Nexus.Api/Features/${name}/${name}Controller.cs` },
      { tpl: 'api/crud-repository.tpl', out: `backend/src/Nexus.Api/Features/${name}/${name}Repository.cs` },
      { tpl: 'api/crud-model.tpl', out: `backend/src/Nexus.Api/Features/${name}/${name}.cs` },
      { tpl: 'api/crud-migration.tpl', out: `backend/src/Nexus.Api/Data/Migrations/Scripts/${timestamp}_Create${name}.sql` },
      { tpl: 'api/crud-test.tpl', out: `backend/tests/Nexus.Tests/Features/${name}Tests.cs` },
      { tpl: 'api/e2e-test.tpl', out: `backend/tests/Nexus.Tests/Features/E2E/${name}E2ETests.cs` }
    ],
    frontend: [
      { tpl: 'frontend/page.tpl', out: `frontend/src/pages/${name}Page.tsx` },
      { tpl: 'frontend/hook.tpl', out: `frontend/src/hooks/use${name}.ts` },
      { tpl: 'frontend/page-test.tpl', out: `frontend/src/pages/${name}Page.test.tsx` }
    ]
  }
};

function scaffold(type) {
  const config = templates[type];
  if (!config) {
    console.error(`Unknown type: ${type}`);
    return;
  }

  [...config.api, ...config.frontend].forEach(item => {
    const tplPath = path.join(__dirname, 'templates', item.tpl);
    let content = fs.readFileSync(tplPath, 'utf8');
    
    content = content.replace(/{{Name}}/g, name);
    content = content.replace(/{{Namespace}}/g, `Nexus.Api.Features.${name}`);
    content = content.replace(/{{TableName}}/g, name.toLowerCase() + "s");
    content = content.replace(/{{Name.toLowerCase\(\)}}/g, name.toLowerCase());

    const outPath = path.join(process.cwd(), item.out);
    const outDir = path.dirname(outPath);

    if (!fs.existsSync(outDir)) {
      fs.mkdirSync(outDir, { recursive: true });
    }

    fs.writeFileSync(outPath, content);
    console.log(`Created: ${item.out}`);
  });

  console.log('\n✅ Full-Stack E2E feature scaffolded successfully!');
  console.log('\nNext steps:');
  console.log(`1. Backend: Register the repository in Program.cs:`);
  console.log(`   builder.Services.AddScoped<${name}Repository>();`);
  console.log(`2. Frontend: Add the route to App.tsx:`);
  console.log(`   <Route path="/${name.toLowerCase()}" element={<ProtectedRoute><${name}Page /></ProtectedRoute>} />`);
  console.log(`3. Run tests:`);
  console.log(`   dotnet test (API + E2E)`);
  console.log(`   cd frontend && npm test (Frontend Unit)`);
}

if (command === 'crud') {
  scaffold('crud');
} else {
  console.log('Only "crud" command is currently implemented in this script.');
}
