<?php
$imports = simplexml_load_file('Xbox360.xml');

@mkdir('modules', 0777);
foreach ($imports->Group as $Group) {
	$name = (string)$Group->Name;
	$f = fopen("modules/{$name}.cs", 'wb');
	fprintf($f, "using System;\n");
	fprintf($f, "using System.Collections.Generic;\n");
	fprintf($f, "using System.IO;\n");
	fprintf($f, "using System.Linq;\n");
	fprintf($f, "using System.Text;\n");
	fprintf($f, "using System.Threading.Tasks;\n");
	fprintf($f, "using CSharpUtils;\n");
	fprintf($f, "using CSharpUtils.Endian;\n");
	fprintf($f, "using CSharpUtils.Extensions;\n");
	fprintf($f, "\n");
	fprintf($f, "namespace Cs360Emu.Hle.Modules\n");
	fprintf($f, "{\n");
	fprintf($f, "	public class {$name}\n");
	fprintf($f, "	{\n");
	foreach ($Group->Entry as $Entry) {
		$id   = (string)$Entry->attributes()->id;
		$type = (string)$Entry->attributes()->type;
		$name = (string)$Entry->attributes()->name;
		
		if ($type == 'var') $type = 'Variable';
		if ($type == 'func') $type = 'Function';
		
		fprintf($f, "		/// <summary>\n");
		fprintf($f, "		/// {$name}\n");
		fprintf($f, "		/// </summary>\n");
		fprintf($f, "		[Export(Id = {$id}, Type = ExportType.{$type})]\n");
		fprintf($f, "		public void {$name}()\n");
		fprintf($f, "		{\n");
		fprintf($f, "			throw(new NotImplementedException());\n");
		fprintf($f, "		}\n");
		fprintf($f, "\n");
	}
	fprintf($f, "	}\n");
	fprintf($f, "}\n");
	fclose($f);
}