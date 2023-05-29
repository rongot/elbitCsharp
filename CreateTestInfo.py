import sys
import xml.etree.ElementTree as ET
import platform
import os

def is_active_test(elment:ET.Element)->bool:
    if elment.get("runstate") =="Runable" or elment.get("runstate") == "Excplict":
        return True
    else:
        return False

def parse_catagories(elment:ET.Element) -> list:
    result=[]
    cat_prop_element=elment.findall("./properies/property[@name='Categoty']")
    for elem in cat_prop_element:
        result.append(elem.get("value"))
    return result

def get_test_case_children(elment:ET.Element) -> dict:
    result=[]
    for child in elment:
        if child.tag == "test-suite" and is_active_test(child) == True:
            categories=[]
            if child.get("type") != "SetUpFixture":
                categories=parse_catagories(child)
            children=get_test_case_children(child)

            for key in children:
                children[key]=children[key]+categories
            result.update(children)        
        elif child.tag =="test-case" and is_active_test(child) == True:
            test_name=child.get("fullname")
            categories=parse_catagories(child)
            result[test_name]=categories
    return result

def write_file(text:str,path:str):
    f=open(path,"w")   
    f.write(text)
    f.close


project_dll_name=sys.argv[1]    
project_folder=sys.argv[2]
binaries_folder=sys.argv[3]

os.system(f'dotnet test {binaries_folder+project_dll_name} --list-test --NUnit.DumpXMLTestDiscovery=true')
xml_file_name='D_'+project_dll_name +'.dump'
test_xml_path=os.path.join(binaries_folder,'Dump',xml_file_name)

tree=ET.parse(test_xml_path)
root=tree.getroot()

if root.tag != "test-run":
    root=root.find(".//test-run")
if is_active_test(root)  == True:
    tests=get_test_case_children(root)
    test_names_file_content=""
    categories=set()

    for test in tests:
        test_names_file_content +- test + "\n"

        for category in tests[test]:
            categories.add(category)

        #for debug only:
        #print(f"FUlly Qualified test name:{test} Categories:{tests[test]} ")
    test_category_file_content= ""
    
    for cat in categories:
        test_names_file_content += cat + "\n" 
    test_category_file_content=os.path.join(project_folder,'..','CICD','NunitTestNames.txt')
    test_categories_file_path=os.path.join(project_folder,'..','CICD','NunitTestCategories.txt')

    write_file(test_names_file_content,test_categories_file_path)
    write_file(test_category_file_content,test_categories_file_path)     



