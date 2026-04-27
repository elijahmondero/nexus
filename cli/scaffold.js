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

function autoRegister() {
  console.log('\n--- Automating Registrations ---');
  // Backend Program.cs
  const programPath = path.join(process.cwd(), 'backend/src/Nexus.Api/Program.cs');
  if (fs.existsSync(programPath)) {
    let programContent = fs.readFileSync(programPath, 'utf8');
    const serviceRegistration = `builder.Services.AddScoped<Nexus.Api.Features.${name}.Repositories.${name}Repository>();`;
    if (!programContent.includes(serviceRegistration)) {
      programContent = programContent.replace(
        'builder.Services.AddScoped<AuthService>();',
        `builder.Services.AddScoped<AuthService>();\n${serviceRegistration}`
      );
      fs.writeFileSync(programPath, programContent);
      console.log(`✅ Auto-registered ${name}Repository in Program.cs`);
    } else {
      console.log(`ℹ️  ${name}Repository already registered in Program.cs`);
    }
  }

  // Frontend App.tsx
  const appPath = path.join(process.cwd(), 'frontend/src/App.tsx');
  if (fs.existsSync(appPath)) {
    let appContent = fs.readFileSync(appPath, 'utf8');
    const importStatement = `import ${name}Page from './pages/${name}Page';`;
    const routeStatement = `<Route\n              path="/${name.toLowerCase()}"\n              element={\n                <ProtectedRoute>\n                  <${name}Page />\n                </ProtectedRoute>\n              }\n            />`;
    
    let changed = false;
    if (!appContent.includes(importStatement)) {
      appContent = appContent.replace(
        "import HomePage from './pages/HomePage';",
        `import HomePage from './pages/HomePage';\n${importStatement}`
      );
      changed = true;
    }
    if (!appContent.includes(`<${name}Page />`)) {
      // Find the last route element before </Routes> and insert there
      appContent = appContent.replace(
        '          </Routes>',
        `            ${routeStatement}\n          </Routes>`
      );
      changed = true;
    }
    
    if (changed) {
      fs.writeFileSync(appPath, appContent);
      console.log(`✅ Auto-registered ${name}Page route in App.tsx`);
    } else {
      console.log(`ℹ️  ${name}Page route already registered in App.tsx`);
    }
  }
}

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
    content = content.replace(/{{Name\.toLowerCase\(\)}}/g, name.toLowerCase());

    const outPath = path.join(process.cwd(), item.out);
    const outDir = path.dirname(outPath);

    if (!fs.existsSync(outDir)) {
      fs.mkdirSync(outDir, { recursive: true });
    }

    fs.writeFileSync(outPath, content);
    console.log(`Created: ${item.out}`);
  });

  autoRegister();

  console.log('\n✅ Full-Stack feature scaffolded and automatically wired up successfully!');
  console.log('\nNext steps:');
  console.log(`1. Review the generated files in backend/src/Nexus.Api/Features/${name} and frontend/src/pages/${name}Page.tsx`);
  console.log(`2. Modify the model and SQL migration script to suit your needs.`);
  console.log(`3. Run tests:`);
  console.log(`   dotnet test (API + E2E)`);
  console.log(`   cd frontend && npm test (Frontend Unit)`);
}

if (command === 'crud') {
  scaffold('crud');
} else {
  console.log('Only "crud" command is currently implemented in this script.');
}
