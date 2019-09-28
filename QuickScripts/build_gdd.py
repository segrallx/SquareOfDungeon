import xlrd
import sys
import os


def col_type(name):
    lx = name.split("_")
    lx.append("good")
    raw_name = lx[1]
    if name.startswith("INT_"):
        return 'int', raw_name
    elif name.startswith("STR_"):
        return 'str', raw_name
    elif name.startswith("FLOAT_"):
        return "float", raw_name
    else:
        return 'str'


def col_value(typ, val):
    if typ == "int":
        return str(int(val))
    elif typ == "str":
        return str(val)
    elif typ == "float":
        return str(val)
    else:
        raise Exception("type err")


def build_sheet(input_path, output_path, sheet):
    list_type = []
    list_data = []
    for i in range(sheet.nrows):
        row = sheet.row_values(i)
        if i == 0:
            # print "head", sheet.row_values(i)
            list_head = []
            for j in row:
                t, h = col_type(j)
                list_type.append(t)
                list_head.append(h)
            list_data.append(list_head)
        else:
            list_row = []
            for index in range(len(row)):
                list_row.append(col_value(list_type[index], row[index]))
            list_data.append(list_row)

    output_file_name = os.path.basename(input_path).split(
        ".")[0] + "_" + sheet.name
    print("export", output_file_name)
    output_path = output_path + "/" + output_file_name + ".txt"
    f = open(output_path, 'w')
    for data in list_data:
        f.writelines(",".join(data) + "\n")
    f.close()


def build_file(input_path, output_path):
    data = xlrd.open_workbook(input_path)
    for sheet in data.sheets():
        build_sheet(input_path, output_path, sheet)


def build_dir(input_path, output_path):
    files = os.listdir(input_path)
    for file in files:
        if file.endswith(".xls"):
            build_file(input_path + "/" + file, output_path)


if __name__ == "__main__":
    if len(sys.argv) != 3:
        print("Usage input_dir output_dir")
        exit(0)

    input_dir = sys.argv[1]
    output_dir = sys.argv[2]
    build_dir(input_dir, output_dir)
